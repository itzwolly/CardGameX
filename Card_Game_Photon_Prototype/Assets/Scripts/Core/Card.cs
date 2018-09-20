using UnityEngine;
using System.Collections.Generic;

public class Card {
    private CardData _data;
    private List<Action> _actions;

    public CardData Data {
        get { return _data; }
    }

    public Card(int pId, string pName, string pDescription, string pActions) {
        _data = new CardData(pId, pName, pDescription, pActions);
        _actions = new List<Action>();

        ConvertActions(pActions);
    }

    public Card(CardData pCardData) {
        _data = pCardData;
        _actions = new List<Action>();

        ConvertActions(_data.ActionsToString);
    }

    private void ConvertActions(string pActions) {
        if (pActions == "") {
            return;
        }
        
        if (pActions.Contains(",")) {
            string[] actions = pActions.Split(',');

            for (int i = 0; i < actions.Length; i++) {
                string actionString = actions[i].Trim();
                Action action = Action.CreateInstance(actionString);
                _actions.Add(action);
            }
        } else {
            string actionString = pActions.Trim();
            Action action = Action.CreateInstance(actionString);
            _actions.Add(action);
        }
    }

    public void ExecuteOnEnter() {
        foreach (Action action in _actions) {
            action.OnEnter();
        }
    }

    public void ExecuteOnExit() {
        foreach (Action action in _actions) {
            action.OnExit();
        }
    }

    public void ExecuteOnStay() {
        foreach (Action action in _actions) {
            action.OnStay();
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
