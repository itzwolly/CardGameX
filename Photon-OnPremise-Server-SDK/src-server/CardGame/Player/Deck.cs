namespace CardGame {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Deck {
        public const int SIZE = 20;
        public const int TURBO_SIZE = 5;

        private const int CARD_DUPLICATE_LIMIT = 3;
        private const int TURBO_CARD_DUPLICATE_LIMIT = 1;

        private static Random _random = new Random();

        private List<Card> _deck; // the deck that's being used to draw cards etc.
        private List<Card> _turbo; // the deck that's being used to draw cards etc.

        public Deck() {
            _deck = new List<Card>();
            _turbo = new List<Card>();
        }

        public Deck(CardInfo[] pRegCards, CardInfo[] pTurboCards) {
            _deck = new List<Card>();
            _turbo = new List<Card>();

            PopulateDeck(pRegCards);
            PopulateTurbo(pTurboCards);
        }


        public void PopulateDeck(CardInfo[] pCards) {
            if (pCards == null) {
                return;
            }

            foreach (CardInfo info in pCards) {
                Card.CardType type = Card.ValidateType(info.type);
                Card card = null;

                switch (type) {
                    case Card.CardType.Spell:
                        card = new SpellCard(info.id, info.name, (info.description == null) ? "" : info.description, info.regcost, info.turbocost, false);
                        break;
                    case Card.CardType.Monster:
                        card = new MonsterCard(info.id, info.name, info.attack, info.health, (info.description == null) ? "" : info.description, info.regcost, info.turbocost, false);
                        break;
                    case Card.CardType.None:
                    default:
                        break;
                }

                AddCard(card);
            }
        }

        public void PopulateTurbo(CardInfo[] pCards) {
            if (pCards == null) {
                return;
            }

            foreach (CardInfo info in pCards) {
                Card.CardType type = Card.ValidateType(info.type);
                Card card = null;

                switch (type) {
                    case Card.CardType.Spell:
                        card = new SpellCard(info.id, info.name, (info.description == null) ? "" : info.description, info.regcost, info.turbocost, true);
                        break;
                    case Card.CardType.Monster:
                        card = new MonsterCard(info.id, info.name, info.attack, info.health, (info.description == null) ? "" : info.description, info.regcost, info.turbocost, true);
                        break;
                    case Card.CardType.None:
                    default:
                        break;
                }

                AddTurboCard(card);
            }
        }

        public void AddCard(Card pCard) {
            if (pCard == null) {
                return;
            }

            int duplicates = GetDuplicateCardCount(pCard);
            if (_deck.Count >= SIZE || duplicates == CARD_DUPLICATE_LIMIT) {
                return;
            }

            _deck.Add(pCard);
        }

        public void AddTurboCard(Card pCard) {
            if (pCard == null) {
                return;
            }

            int duplicates = GetTurboDuplicateCardCount(pCard);
            if (_turbo.Count >= TURBO_SIZE || duplicates == TURBO_CARD_DUPLICATE_LIMIT) {
                return;
            }

            _turbo.Add(pCard);
        }

        public void RemoveCard(Card pCard) {
            if (pCard == null) {
                return;
            }

            Card clone = pCard;
            _deck.Remove(pCard);
            // return clone;
        }

        public void RemoveTurboCard(Card pCard) {
            if (pCard == null) {
                return;
            }

            Card clone = pCard;
            _turbo.Remove(pCard);
            // return clone;
        }

        public Card DrawCard() {
            if (_deck.Count > 0) {
                Shuffle(_deck); // Shuffle the deck

                Card copy = _deck[0]; // Create clone to return, because we're deleting later.
                _deck.RemoveAt(0); // remove existing card from deck

                return copy; // Return copy.
            }
            return null;
        }

        public Card DrawCard(int pPosition) {
            if (_deck.Count > pPosition - 1) {
                Shuffle(_deck); // Shuffle the deck

                Card copy = _deck[pPosition - 1]; // Create clone to return, because we're deleting later.
                _deck.RemoveAt(pPosition - 1); // remove existing card from deck

                return copy; // Return copy.
            }
            return null;
        }

        public Card[] DrawCards(int pAmount) {
            Card[] cardsToDraw = new Card[pAmount];

            if (pAmount > 1) {
                for (int i = 0; i < pAmount; i++) {
                    if (_deck.Count == 0) {
                        break;
                    }

                    Shuffle(_deck); // Shuffle the deck

                    Card copy = _deck[0]; // Create clone to return, because we're deleting later.
                    _deck.RemoveAt(0); // remove existing card from deck
                    cardsToDraw[i] = copy;
                }
                return cardsToDraw;
            }

            cardsToDraw[0] = DrawCard();
            return cardsToDraw;
        }

        public Card GetCard(int pId) {
            return _deck.Find(o => o.Id == pId);
        }

        public Card GetTurboCard(int pId) {
            return _turbo.Find(o => o.Id == pId);
        }

        public List<Card> GetCards() {
            return _deck;
        }

        public List<Card> GetCards(Type pType) {
            return _deck.Where(o => o.GetType() == pType).ToList();
        }

        public List<Card> GetTurboCards() {
            return _turbo;
        }

        public int GetDuplicateCardCount(Card pCard) {
            if (pCard == null) {
                return 0;
            }
            return _deck.Where(o => o.Id == pCard.Id).Count();
        }

        public int GetTurboDuplicateCardCount(Card pCard) {
            if (pCard == null) {
                return 0;
            }
            return _turbo.Where(o => o.Id == pCard.Id).Count();
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