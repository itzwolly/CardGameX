using System;

namespace CardGame {

    public class Player {
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
                    _currentTurbo = MAX_MANA;
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
            BoardSide = new BoardSide();
        }
    }
}
