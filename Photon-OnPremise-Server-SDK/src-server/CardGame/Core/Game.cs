using System;
using System.Collections.Generic;

namespace CardGame {
    
    public class Game {
        public readonly PlayerState PlayerState;
        public readonly BoardState BoardState;
        public bool Started = false;

        // Constructor
        public Game(int pMaxPlayers) {
            PlayerState = new PlayerState(pMaxPlayers);
            BoardState = new BoardState(PlayerState);
        }

        public Player AddPlayer(int pActorNr, string pUserId, Deck pDeck) {
            return PlayerState.AddPlayer(pActorNr, pUserId, pDeck);
        }
    }

}

