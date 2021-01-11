using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
    public class BoardSide {
        private Player _owner;

        public readonly BoardSlot[] Slots;

        public BoardSide(Player pOwner) {
            _owner = pOwner;
            Slots = new BoardSlot[BoardState.BOARD_SIDE_SIZE];

            for (int i = 0; i < Slots.Length; i++) {
                Slots[i] = new BoardSlot(i, null, _owner);
            }
        }

        public bool Occupy(int pIndex, MonsterCard pMonster) {
            BoardSlot slot = Slots[pIndex];
            if (slot.Monster == null) {
                slot.UpdateMonster(pMonster);
                return true;
            }
            return false;
        }

        public bool KillMonster(MonsterCard pMonster, bool pRemoveEnhancements = true) {
            if (pMonster.BoardIndex >= Slots.Length || pMonster.BoardIndex < 0) {
                return false;
            }
            BoardSlot slot = Slots[pMonster.BoardIndex];
            if (pRemoveEnhancements) {
                Game.Instance.BoardState.RemoveAllEnhancements(pMonster);
            }
            slot.ClearMonster();
            return true;
        }

        public MonsterCard GetMonsterByIndex(int pIndex) {
            if (pIndex < 0 || pIndex >= Slots.Length) {
                return null;
            }
            return Slots[pIndex].Monster;
        }

        public BoardSlot GetSlotByIndex(int pIndex) {
            if (pIndex < 0 || pIndex >= Slots.Length) {
                return null;
            }
            return Slots[pIndex];
        }

        public List<MonsterCard> GetMonsters() {
            List<MonsterCard> monsters = new List<MonsterCard>();
            foreach (BoardSlot slot in Slots) {
                if (slot.Monster != null) {
                    monsters.Add(slot.Monster);
                }
            }
            return monsters;
        }
    }
}

