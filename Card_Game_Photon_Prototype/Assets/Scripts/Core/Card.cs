using UnityEngine;
using System.Collections.Generic;

public class Card {
    private int _id;
    private string _name;
    private string _description;
    private List<Action> _actions;

    public Card(int pId, string pName, string pDescription, List<Action> pActions) {
        _id = pId;
        _name = pName;
        _description = pDescription;
        _actions = pActions;
    }

    public void Execute() {
        foreach (Action action in _actions) {
            action.Execute();
        }
    }

    public override string ToString() {
        string s = "Card data: ";
        s += _id + ", " + _name + ", " + _description + ", ";

        for (int i = 0; i < _actions.Count; i++) {
            s += _actions[i].GetType() + ((i == _actions.Count) ? "" : " | ");
        } 

        return s;
    }
}
