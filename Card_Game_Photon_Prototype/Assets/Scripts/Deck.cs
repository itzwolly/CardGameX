using System.Collections.Generic;
using UnityEngine;

public class Deck {
    private int _id;
    private string _name;
    private List<Card> _cards;
    private static System.Random _random = new System.Random();

    public Deck(int pId, string pName) {
        _id = pId;
        _name = pName;
        _cards = new List<Card>();
    }

    public void AddCard(Card pCard) {
        if (_cards.Count >= Config.DECK_SIZE) {
            return;
        }

        _cards.Add(pCard);
        Debug.Log("Deck now has: " + _cards.Count + " amount of cards.");
    }

    public Card DrawCard() {
        if (_cards.Count > 0) {
            Shuffle(_cards); // Shuffle the deck

            Card copy = _cards[0]; // Create clone to return, because we're deleting later.
            _cards.RemoveAt(0); // remove existing card from deck

            return copy; // Return top card.
        }
        return null;
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
