using System;
using System.Collections.Generic;

namespace CardGame {

    public class Hand {
        public const int MAX_CARDS = 5;

        public List<Card> Cards { get; private set; }

        // Constructor
        public Hand() {
            Cards = new List<Card>();
        }

        public void AddCard(Game pGame, Card pCard) {
            pGame.RegisterFunctions(pCard);
            Cards.Add(pCard);
        }

        public Card GetCard(int pCardId) {
            Card card = Cards.Find(o => o.Id == pCardId);
            if (card != null) {
                return card;
            }
            return null;
        }
    }
}


