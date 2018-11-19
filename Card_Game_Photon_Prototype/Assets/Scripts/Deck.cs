using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text;

public class Deck {
    private DeckData _data;
    private List<Card> _deck;
    private List<Card> _turbo;

    public DeckData Data {
        get { return _data; }
    }
    public List<Card> Cards {
        get { return _deck; }
        set { _deck = value; }
    }
    public List<Card> Turbo {
        get { return _turbo; }
        set { _turbo = value; }
    }

    public Deck(DeckData pData) {
        _data = pData;
        _deck = new List<Card>();
        _turbo = new List<Card>();
    }

    public void AddRegCard(Card pCard) {
        if (pCard == null) {
            Debug.Log("The card you are trying to add is null!");
            return;
        }
        
        int duplicates = GetRegularDeckDuplicateCardCount(pCard);

        if (_deck.Count >= Config.DECK_REGULAR_SIZE || duplicates == Config.DECK_REG_CARD_LIMIT) {
            Debug.Log("Exceeded amount of duplicates/cards in a deck");
            return;
        }
        
        _deck.Add(pCard);
        CardCollectionList.Instance.AddCardEntry(pCard.Data, duplicates, false);
        Debug.Log("Regular deck now has: " + _deck.Count + " amount of cards.");
    }

    public void AddTurboCard(Card pCard) {
        if (pCard == null) {
            Debug.Log("The card you are trying to add is null!");
            return;
        }

        int duplicates = GetTurboDeckDuplicateCardCount(pCard);

        if (_turbo.Count >= Config.DECK_TURBO_SIZE || duplicates == Config.DECK_TURBO_CARD_LIMIT) {
            Debug.Log("Exceeded amount of duplicates/cards in a deck");
            return;
        }

        _turbo.Add(pCard);
        CardCollectionList.Instance.AddCardEntry(pCard.Data, duplicates, true);
        Debug.Log("Turbo deck now has: " + _turbo.Count + " amount of cards.");
    }

    public void RemoveRegCard(Card pCard) {
        if (pCard == null) {
            Debug.Log("The card you are trying to remove is null!");
            return;
        }

        Card clone = pCard;
        _deck.Remove(pCard);
        CardCollectionList.Instance.RemoveCardEntry(clone.Data, GetRegularDeckDuplicateCardCount(clone), false);
        Debug.Log("Regular deck now has: " + _deck.Count + " amount of cards.");
    }

    public void RemoveTurboCard(Card pCard) {
        if (pCard == null) {
            Debug.Log("The card you are trying to remove is null!");
            return;
        }

        Card clone = pCard;
        _turbo.Remove(pCard);
        CardCollectionList.Instance.RemoveCardEntry(clone.Data, GetTurboDeckDuplicateCardCount(clone), true);
        Debug.Log("Turbo deck now has: " + _turbo.Count + " amount of cards.");
    }

    public Card GetRegCard(CardData pCardData) {
        return _deck.FirstOrDefault(o => o.Data.Id == pCardData.Id);
    }

    public Card GetTurboCard(CardData pCardData) {
        return _turbo.FirstOrDefault(o => o.Data.Id == pCardData.Id);
    }

    public List<Card> GetRegCards() {
        return _deck;
    }

    public List<Card> GetTurboCards() {
        return _turbo;
    }

    public Card GetRegCardById(int pId) {
        return _deck.FirstOrDefault(o => o.Data.Id == pId);
    }

    public Card GetTurboCardById(int pId) {
        return _turbo.FirstOrDefault(o => o.Data.Id == pId);
    }

    public int GetRegularDeckDuplicateCardCount(Card pCard) {
        if (pCard == null) {
            Debug.Log("The card you are trying to get the duplicate of is null. Returning 0...");
            return 0;
        }

        return _deck.Where(o => o.Data.Id == pCard.Data.Id).Count();
    }

    public int GetTurboDeckDuplicateCardCount(Card pCard) {
        if (pCard == null) {
            Debug.Log("The card you are trying to get the duplicate of is null. Returning 0...");
            return 0;
        }

        return _turbo.Where(o => o.Data.Id == pCard.Data.Id).Count();
    }

    public string UpdateRegDeckCode() {
        string code = "";

        for (int i = 0; i < _deck.Count; i++) {
            code += _deck[i].Data.Id + ((i == _deck.Count - 1) ? "" : ",");
        }
        _data.DeckCode = Serialize(code);
        return _data.DeckCode;
    }

    public string UpdateTurboCode() {
        string code = "";

        for (int i = 0; i < _turbo.Count; i++) {
            code += _turbo[i].Data.Id + ((i == _turbo.Count - 1) ? "" : ",");
        }
        _data.TurboCode = Serialize(code);
        return _data.TurboCode;
    }

    private static string Serialize(string pPlainTextDeckCode) {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(pPlainTextDeckCode);
        string result = Convert.ToBase64String(plainTextBytes);
        return result;
    }
}