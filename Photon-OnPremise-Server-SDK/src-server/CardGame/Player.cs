using System;

namespace CardGame {

    public class Player {
        private const int STARTING_HEALTH = 10;

        public int ActorNr { get; private set; }
        public string UserId { get; private set; }
        public Deck Deck { get; private set; }
        public Hand Hand { get; set; }

        private int _health = STARTING_HEALTH;

        public int Health {
            get { return _health; }
            set {
                _health = value;

                if (_health < 0) {
                    _health = 0;
                }
            }
        }

        // Constructor
        public Player(int pActorNr, string pUserId, Deck pCurrentDeck) {
            ActorNr = pActorNr;
            UserId = pUserId;
            Deck = pCurrentDeck;
        }
    }
}
