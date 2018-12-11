using System;
using System.Collections.Generic;

namespace CardGame {
    public class TargetCollection {
        public readonly IInteractable Owner;
        public readonly TargetType Type;

        // Constructor
        public TargetCollection(IInteractable pOwner, TargetType pType) {
            Owner = pOwner;
            Type = pType;
        }

        public static TargetType SerializeTargetType(string pTargetType) {
            TargetType targetType;
            bool parsed = Enum.TryParse(pTargetType, out targetType);
            if (parsed) {
                return targetType;
            }
            return TargetType.None;
        }

        public List<IInteractable> GetTargets() {
            List<IInteractable> targets = new List<IInteractable>();
            switch (Type) {
                case TargetType.Self: {
                        targets.Add(Owner);
                        break;
                    }
                case TargetType.Others: {
                        List<Player> players = Game.Instance.PlayerState.GetPlayers();
                        for (int i = 0; i < players.Count; i++) {
                            Player player = players[i];
                            for (int j = 0; j < player.BoardSide.Slots.Length; j++) {
                                MonsterCard monster = player.BoardSide.GetMonsterByIndex(j);
                                if (monster != Owner) {
                                    targets.Add(monster);
                                }
                            }
                        }
                        break;
                    }
                    
                case TargetType.All: {
                        List<Player> players = Game.Instance.PlayerState.GetPlayers();
                        for (int i = 0; i < players.Count; i++) {
                            Player player = players[i];
                            for (int j = 0; j < player.BoardSide.Slots.Length; j++) {
                                MonsterCard monster = player.BoardSide.GetMonsterByIndex(j);
                                if (monster != null) {
                                    targets.Add(monster);
                                }
                            }
                            targets.Add(player);
                        }
                        break;
                    }
                case TargetType.All_Monsters: {
                        List<Player> players = Game.Instance.PlayerState.GetPlayers();
                        for (int i = 0; i < players.Count; i++) {
                            Player player = players[i];
                            for (int j = 0; j < player.BoardSide.Slots.Length; j++) {
                                MonsterCard monster = player.BoardSide.GetMonsterByIndex(j);
                                if (monster != null) {
                                    targets.Add(monster);
                                }
                            }
                        }
                        break;
                    }
                case TargetType.Adjacent: {
                        Player player = Game.Instance.PlayerState.GetPlayerById(Owner.GetOwnerId());
                        BoardSide side = player.BoardSide;
                        
                        MonsterCard left = side.GetMonsterByIndex(Owner.GetBoardIndex() - 1);
                        MonsterCard right = side.GetMonsterByIndex(Owner.GetBoardIndex() + 1);

                        if (left != null) {
                            targets.Add(left);
                        }
                        if (right != null) {
                            targets.Add(right);
                        }
                        break;
                    }
                case TargetType.Enemy_Board: {
                        Player player = Game.Instance.PlayerState.Opponent;
                        for (int i = 0; i < player.BoardSide.Slots.Length; i++) {
                            MonsterCard monster = player.BoardSide.GetMonsterByIndex(i);
                            if (monster != null) {
                                targets.Add(monster);
                            }
                        }
                        break;
                    }
                case TargetType.Player_Board: {
                        Player player = Game.Instance.PlayerState.GetActivePlayer();
                        for (int i = 0; i < player.BoardSide.Slots.Length; i++) {
                            MonsterCard monster = player.BoardSide.GetMonsterByIndex(i);
                            if (monster != null) {
                                targets.Add(monster);
                            }
                        }
                        break;
                    }
                case TargetType.None:
                default:
                    break;
            }
            return targets;
        }

        public enum TargetType {
            None,
            Self,
            Others,
            All,
            All_Monsters,
            Adjacent,
            Enemy_Board,
            Player_Board,
        }
    }
}

