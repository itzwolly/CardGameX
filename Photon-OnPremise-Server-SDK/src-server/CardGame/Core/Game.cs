using NLua;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
    public sealed class Game {
        private const int MAX_PLAYERS = 2;

        private static readonly Lazy<Game> lazy = new Lazy<Game>(() => new Game(MAX_PLAYERS));
        public static Game Instance { get { return lazy.Value; } }

        public readonly PlayerState PlayerState;
        public readonly BoardState BoardState;
        public readonly Lua MonsterDefaultBehaviour;
        public readonly Lua SpellDefaultBehaviour;
        public bool Started;
        
        // Constructor
        private Game(int pMaxPlayers) {
            Started = false;
            PlayerState = new PlayerState(pMaxPlayers);
            BoardState = new BoardState();
            MonsterDefaultBehaviour = LuaHelper.DoFile(LuaHelper.GetScriptFolder() + LuaHelper.MONSTER_DEFAULT_SCRIPT + LuaHelper.FILE_TYPE, this);
            SpellDefaultBehaviour = LuaHelper.DoFile(LuaHelper.GetScriptFolder() + LuaHelper.SPELL_DEFAULT_SCRIPT + LuaHelper.FILE_TYPE, this);
        }

        public Player AddPlayer(int pActorNr, string pUserId, Deck pDeck) {
            Player player = PlayerState.AddPlayer(pActorNr, pUserId, pDeck);
            if (player != null) {
                RegisterFunctions((player as IScriptable).GetBehaviour());
            }
            return player;
        }

        public void RegisterFunctions(Lua pBehaviour) {
            pBehaviour.RegisterFunction("AddEnhancement", this, typeof(Game).GetMethod("AddEnhancement"));
            pBehaviour.RegisterFunction("GetNewestEnhancementValue", this, typeof(Game).GetMethod("GetNewestEnhancementValue"));
            pBehaviour.RegisterFunction("AddAuraEnhancement", this, typeof(Game).GetMethod("AddAuraEnhancement"));
            pBehaviour.RegisterFunction("CancelAuras", this, typeof(Game).GetMethod("CancelAuras"));
            pBehaviour.RegisterFunction("GetTargetCollection", this, typeof(Game).GetMethod("GetTargetCollection"));
            pBehaviour.RegisterFunction("IsEnemy", this, typeof(Game).GetMethod("IsEnemy"));
            pBehaviour.RegisterFunction("IsMonster", this, typeof(Game).GetMethod("IsMonster"));
            pBehaviour.RegisterFunction("GetMonsterUsingIndex", this, typeof(Game).GetMethod("GetMonsterUsingIndex"));
        }

        public void AddEnhancement(IInteractable pOwner, string pEnhancement, int pValue) {
            BoardState.BoardEnhancements enhancement = BoardState.Serialize(pEnhancement);
            if (enhancement == BoardState.BoardEnhancements.None) {
                return; // no enhancement found..
            }
            BoardState.AddEnhancement(pOwner, enhancement, pValue);
        }

        public object GetNewestEnhancementValue(IInteractable pOwner, string pEnhancement) {
            BoardState.BoardEnhancements enhancement = BoardState.Serialize(pEnhancement);
            if (enhancement == BoardState.BoardEnhancements.None) {
                return null; // no enhancement found..
            }
            return BoardState.GetNewestEnhancementValue(pOwner, enhancement);
        }

        public void AddAuraEnhancement(IInteractable pOwner, string pTargetType, string pEnhancement, int pValue) {
            BoardState.BoardEnhancements enhancement = BoardState.Serialize(pEnhancement);
            if (enhancement == BoardState.BoardEnhancements.None) {
                return; // no enhancement found..
            }
            TargetCollection.TargetType targetType = TargetCollection.SerializeTargetType(pTargetType);
            if (targetType == TargetCollection.TargetType.None) {
                return; // no target type found..
            }
            BoardState.AddAuraEnhancement(pOwner, enhancement, targetType, pValue);
        }

        public IInteractable GetMonsterUsingIndex(int pOwnerId, int pIndex) {
            if (PlayerState.IsPlayerId(pOwnerId)) {
                Player player = PlayerState.GetPlayerById(pOwnerId);
                if (player != null) {
                    return player.BoardSide.GetMonsterByIndex(pIndex);
                }
            }
            return null;
        }

        public LuaTable GetTargetCollection(IInteractable pOwner, string pTargetType) {
            TargetCollection.TargetType targetType = TargetCollection.SerializeTargetType(pTargetType);
            if (targetType == TargetCollection.TargetType.None) {
                return null; // no target type found..
            }

            TargetCollection collection = new TargetCollection(pOwner, targetType);
            LuaTable table = CreateTable((pOwner as IScriptable).GetBehaviour());

            List<IInteractable> targets = collection.GetTargets();
            for (int i = 0; i < targets.Count; i++) {
                IInteractable target = targets[i];
                table[i] = target;
            }
            return table;
        }

        public void CancelAuras(IInteractable pOwner) {
            BoardState.CleanAuras(pOwner);
        }

        public LuaTable CreateTable(Lua pLua) {
            return (LuaTable) pLua.DoString("return {}")[0];
        }

        public bool IsEnemy(IInteractable pOwner) {
            if (PlayerState.IsPlayerId(pOwner.GetId())) {
                if (pOwner.GetId() == PlayerState.GetActivePlayer().GetId()) {
                    return false;
                } else {
                    return true;
                }
            } else {
                if (pOwner.GetOwnerId() == PlayerState.GetActivePlayer().GetId()) {
                    return false;
                } else {
                    return true;
                }
            }
        }

        public bool IsMonster(IInteractable pOwner) {
             if (PlayerState.IsPlayerId(pOwner.GetId())) {
                return false;
             } else {
                Card card = pOwner as Card;
                return card.IsMonster();
            }
        }
    }
}

