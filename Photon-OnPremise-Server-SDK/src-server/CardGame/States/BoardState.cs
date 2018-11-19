using System;
using System.Collections.Generic;


namespace CardGame {

    public class BoardState {
        public const int BOARD_SIDE_SIZE = 5;

        public readonly PlayerState PlayerState;

        public BoardState(PlayerState pPlayerState) {
            PlayerState = pPlayerState;
        }
    }

}
