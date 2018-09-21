using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Deck {
    private int _id;
    private string _name;
    private List<Card> _deck;
    private static System.Random _random = new System.Random();

    public Deck(int pId, string pName) {
        _id = pId;
        _name = pName;
        _deck = new List<Card>();
    }

    public void AddCard(Card pCard) {
        if (pCard == null) {
            Debug.Log("The card you are trying to add is null!");
            return;
        }
        
        int duplicates = GetDuplicateCardCount(pCard);

        if (_deck.Count >= Config.DECK_SIZE || duplicates == Config.CARD_LIMIT) {
            Debug.Log("Exceeded amount of duplicates/cards in a deck");
            return;
        }
        
        _deck.Add(pCard);
        CardCollectionList.Instance.AddCardEntry(pCard, duplicates);

        Debug.Log("Deck now has: " + _deck.Count + " amount of cards.");
    }

    public void RemoveCard(Card pCard) {
        if (pCard == null) {
            Debug.Log("The card you are trying to remove is null!");
            return;
        }

        Card clone = pCard;
        _deck.Remove(pCard);
        CardCollectionList.Instance.RemoveCardEntry(clone, GetDuplicateCardCount(clone));
        Debug.Log("Deck now has: " + _deck.Count + " amount of cards.");
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

    public Card GetCard(CardData pData) {
        return _deck.Find(o => o.Data.Id == pData.Id);
    }

    public Card GetCard(int pId) {
        return _deck.Find(o => o.Data.Id == pId);
    }

    public List<Card> GetCards() {
        return _deck;
    }

    public int GetDuplicateCardCount(Card pCard) {
        if (pCard == null) {
            Debug.Log("The card you are trying to get the duplicate of is null. Returning 0...");
            return 0;
        }

        return _deck.Where(o => o.Data.Id == pCard.Data.Id).Count();
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
