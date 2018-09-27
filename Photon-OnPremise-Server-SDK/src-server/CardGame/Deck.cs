namespace CardGame {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Deck {
        public const int SIZE = 3;
        private const int CARD_DUPLICATE_LIMIT = 3;
        private static Random _random = new Random();

        private List<Card> _startingDeck; // starting deck
        private List<Card> _localDeck; // the deck that's being used to draw cards etc.

        public Deck() {
            _startingDeck = new List<Card>();
            _localDeck = new List<Card>();
        }

        public void AddCard(Card pCard) {
            if (pCard == null) {
                return;
            }

            int duplicates = GetDuplicateCardCount(pCard);
            if (_localDeck.Count >= SIZE || duplicates == CARD_DUPLICATE_LIMIT) {
                return;
            }

            _localDeck.Add(pCard);
            _startingDeck.Add(pCard);
        }

        public void RemoveCard(Card pCard) {
            if (pCard == null) {
                return;
            }

            Card clone = pCard;
            _localDeck.Remove(pCard);
        }

        public Card DrawCard() {
            if (_localDeck.Count > 0) {
                Shuffle(_localDeck); // Shuffle the deck

                Card copy = _localDeck[0]; // Create clone to return, because we're deleting later.
                _localDeck.RemoveAt(0); // remove existing card from deck

                return copy; // Return copy.
            }
            return null;
        }

        public Card DrawCard(int pPosition) {
            if (_localDeck.Count > pPosition - 1) {
                Shuffle(_localDeck); // Shuffle the deck

                Card copy = _localDeck[pPosition - 1]; // Create clone to return, because we're deleting later.
                _localDeck.RemoveAt(pPosition - 1); // remove existing card from deck

                return copy; // Return copy.
            }
            return null;
        }

        public Card[] DrawCards(int pAmount) {
            Card[] cardsToDraw = new Card[pAmount];

            if (pAmount > 1) {
                for (int i = 0; i < pAmount; i++) {
                    if (_localDeck.Count == 0) {
                        break;
                    }

                    Shuffle(_localDeck); // Shuffle the deck

                    Card copy = _localDeck[0]; // Create clone to return, because we're deleting later.
                    _localDeck.RemoveAt(0); // remove existing card from deck
                    cardsToDraw[i] = copy;
                }
                return cardsToDraw;
            }

            cardsToDraw[0] = DrawCard();
            return cardsToDraw;
        }

        public Card GetCard(int pId) {
            return _localDeck.Find(o => o.Id == pId);
        }

        public List<Card> GetCards() {
            return _localDeck;
        }

        public int GetDuplicateCardCount(Card pCard) {
            if (pCard == null) {
                return 0;
            }
            return _localDeck.Where(o => o.Id == pCard.Id).Count();
        }

        private static void Shuffle<T>(IList<T> pList) {
            int n = pList.Count;
            while (n > 1) {
                n--;
                int k = _random.Next(n + 1);
                T value = pList[k];
                pList[k] = pList[n];
                pList[n] = value;
            }
        }
    }
}