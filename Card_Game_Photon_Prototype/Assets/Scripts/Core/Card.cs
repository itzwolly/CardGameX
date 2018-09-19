using UnityEngine;
using System.Collections.Generic;

public class Card {
    private CardData _data;
    private List<Action> _actions;
    private GameObject _collectionEntry;

    public CardData Data {
        get { return _data; }
    }
    public GameObject CollectionEntry {
        get { return _collectionEntry; }
        set { _collectionEntry = value; }
    }

    public Card(int pId, string pName, string pDescription, List<Action> pActions) {
        _data = new CardData(pId, pName, pDescription, /*pActions*/ "");
    }

    public Card(int pId, string pName, string pDescription, string pActions) {
        _data = new CardData(pId, pName, pDescription, pActions);
    }

    public Card(CardData pCardData) {
        _data = pCardData;
    }

    public void Execute() {
        foreach (Action action in _actions) {
            action.Execute();
        }
    }

    public override string ToString() {
        string s = "Card data: ";
        s += _data.Id + ", " + _data.Name + ", " + _data.Description + ", ";

        if (_actions != null) {
            for (int i = 0; i < _actions.Count; i++) {
                s += _actions[i].GetType() + ((i == _actions.Count) ? "" : " | ");
            }
        } else {
            s += "null";
        }

        return s;
    }
}
