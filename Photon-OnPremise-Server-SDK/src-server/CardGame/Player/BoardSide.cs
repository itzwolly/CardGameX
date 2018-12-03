using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
    public class BoardSide {
        public readonly MonsterCard[] Slots;
        private Dictionary<KeyValuePair<int, BoardEnhancements>, KeyValuePair<object, object>> _enhancements;
        private Player _owner;

        public BoardSide(Player pOwner) {
            _owner = pOwner;
            Slots = new MonsterCard[BoardState.BOARD_SIDE_SIZE];
            _enhancements = new Dictionary<KeyValuePair<int, BoardEnhancements>, KeyValuePair<object, object>>();
        }

        public bool Occupy(int pIndex, MonsterCard pMonster) {
            if (Slots[pIndex] == null) {
                Slots[pIndex] = pMonster;
                return true;
            }
            return false;
        }

        public bool KillMonster(MonsterCard pMonster) {
            if (pMonster.BoardIndex >= Slots.Length || pMonster.BoardIndex < 0) {
                return false;
            }
            RemoveEnhancements(pMonster.Id);
            Slots[pMonster.BoardIndex] = null;
            return true;
        }

        public void AddOrModifyEnhancement(int pId, int pBoardIndex, BoardEnhancements pEnhancement, object pValue, object pOriginalValue) {
            KeyValuePair<int, BoardEnhancements> key = new KeyValuePair<int, BoardEnhancements>(pId, pEnhancement);
            if (_enhancements.ContainsKey(key)) {
                _enhancements[key] = new KeyValuePair<object, object>(pValue, pOriginalValue);
            } else {
                _enhancements.Add(key, new KeyValuePair<object, object>(pValue, pOriginalValue));
            }

            Execute(pId, pBoardIndex, pEnhancement);
        }

        public bool RemoveEnhancement(int pId, BoardEnhancements pEnhancement) {
            KeyValuePair<int, BoardEnhancements> key = new KeyValuePair<int, BoardEnhancements>(pId, pEnhancement);
            bool remove = _enhancements.Remove(key);
            return remove;
        }

        public void RemoveEnhancements(int pId) {
            var itemsToRemove = _enhancements.Where(o => o.Key.Key == pId).ToArray();

            foreach (var item in itemsToRemove) {
                _enhancements.Remove(item.Key);
            }
        }

        public object GetEnhancementValue(int pId, BoardEnhancements pEnhancement) {
            KeyValuePair<int, BoardEnhancements> key = new KeyValuePair<int, BoardEnhancements>(pId, pEnhancement);
            if (_enhancements.ContainsKey(key)) {
                object obj = _enhancements[key].Key;
                return obj;
            }
            return null;
        }

        public void Execute(int pId, int pIndex, BoardEnhancements pEnhancement) {
            switch (pEnhancement) {
                case BoardEnhancements.Set_Health: {
                        // set temporary health of the target
                        KeyValuePair<int, BoardEnhancements> key = new KeyValuePair<int, BoardEnhancements>(pId, pEnhancement);
                        int health = (int) _enhancements[key].Key;
                        int originalHealth = (int) _enhancements[key].Value;

                        if (pId == _owner.ActorNr) {
                            _owner.SetHealth(health);
                        } else {
                            MonsterCard monster = Slots[pIndex];
                            if (monster != null && monster.Id == pId) {
                                monster.SetHealth(health);
                            } else {
                                return; // something went wrong..
                            }
                        }
                    }
                    break;
                case BoardEnhancements.Can_Attack:
                case BoardEnhancements.Death:
                case BoardEnhancements.None:
                default:
                    break;
            }
        }

        public enum BoardEnhancements {
            // Termination
            None = 0x00,
            // Basics
            Set_Health = 0x01,
            // Rush
            Can_Attack = 0x02,
            // Death
            Death = 0x03,
            // Shiel up
            Has_Single_Shield = 0x04,

        }
    }
}

