using System;
using System.Collections.Generic;

namespace CardGame {

    public class Hand {
        public List<Card> Cards { get; private set; }

        // Constructor
        public Hand() {
            Cards = new List<Card>();
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


