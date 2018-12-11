using NLua;
using System;

namespace CardGame {

    public class Player : IInteractable, IScriptable {
        public const int STARTING_HEALTH = 10;
        public const int STARTING_MANA = 0; // It's 0 because the very first turn you get +1 mana
        public const int STARTING_TURBO = 0; // It's 0 because the very first turn you get +1 turbo

        private const int MAX_MANA = 10;
        private const int MAX_TURBO = 10;

        private int _health = STARTING_HEALTH;
        private int _currentMana = STARTING_MANA;
        private int _currentTurbo = STARTING_TURBO;

        private int _totalMana = STARTING_MANA;
        private int _totalTurbo = STARTING_TURBO;

        public readonly int ActorNr;
        public readonly string UserId;
        public readonly Deck Deck;
        public readonly Hand Hand;
        public readonly BoardSide BoardSide;
        public readonly Lua Behaviour;

        public bool IsDead = false;

        public int Health {
            get { return _health; }
            set {
                _health = value;

                if (_health < 0) {
                    _health = 0;
                }
            }
        }
        public int CurrentMana {
            get { return _currentMana; }
            set {
                if (_currentMana >= MAX_MANA) {
                    return;
                }
                _currentMana = value;
            }
        }
        public int CurrentTurbo {
            get { return _currentTurbo; }
            set {
                if (_currentTurbo >= MAX_TURBO) {
                    return;
                }
                _currentTurbo = value;
            }
        }
        public int TotalMana {
            get { return _totalMana; }
            set {
                if (_totalMana >= MAX_MANA) {
                    _currentMana = MAX_MANA;
                    return;
                }
                _totalMana = value;
                _currentMana = _totalMana;
            }
        }
        public int TotalTurbo {
            get { return _totalTurbo; }
            set {
                if (_totalTurbo >= MAX_TURBO) {
                    _currentTurbo = MAX_TURBO;
                    return;
                }
                _totalTurbo = value;
                _currentTurbo = _totalTurbo;
            }
        }

        // Constructor
        public Player(int pActorNr, string pUserId, Deck pCurrentDeck) {
            ActorNr = pActorNr;
            UserId = pUserId;
            Deck = pCurrentDeck;
            Hand = new Hand();
            BoardSide = new BoardSide(this);
            Behaviour = LuaHelper.DoFile(LuaHelper.GetScriptFolder() + LuaHelper.PLAYER_SCRIPT + LuaHelper.FILE_TYPE, Game.Instance);
        }
        
        public int GetId() {
            return ActorNr;
        }

        public int? GetHealth() {
            return Health;
        }

        // TODO: Maybe add later when we have weapons or smthing idk.
        public int? GetAttack() {
            return 0;
        }

        public void SetHealth(int pAmount) {
            if (pAmount < 0) {
                Health = 0;
                return;
            }
            Health = pAmount;
        }

        public void SetAttack(int pAmount) {
            // TODO: Implement at some point..
        }

        public int GetOwnerId() {
            return -1;
        }

        public int GetBoardIndex() {
            return -1;
        }

        public Lua GetBehaviour() {
            return Behaviour;
        }

        public string GetName() {
            return UserId;
        }

        public object[] CallFunction(string pFunction, params object[] pArgs) {
            LuaFunction function = Behaviour.GetFunction(pFunction);
            if (function != null) {
                return function.Call(pArgs);
            } else {
                function = Game.Instance.MonsterDefaultBehaviour.GetFunction(LuaHelper.MONSTER_DEFAULT_NAMESPACE + pFunction);
                return function.Call(pArgs);
            }
        }
    }
}
