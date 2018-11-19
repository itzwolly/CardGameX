using System;
using System.Collections.Generic;

namespace CardGame {
    public class BoardSide {
        public readonly int[] Slots;

        public BoardSide() {
            Slots = new int[BoardState.BOARD_SIDE_SIZE];
        }

        public bool Occupy(int pIndex, int pMonsterId) {
            if (Slots[pIndex] == 0) {
                Slots[pIndex] = pMonsterId;
                return true;
            }
            return false;
        }
    }
}

