using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardData {
    private int _id;
    private string _name;
    private string _description;
    private string _actions;

    public int Id {
        get { return _id; }
        set { _id = value; }
    }
    public string Name {
        get { return _name; }
        set { _name = value; }
    }
    public string Description {
        get { return _description; }
        set { _description = value; }
    }
    public string Actions {
        get { return _actions; }
        set { _actions = value; }
    }

    public CardData(int pId, string pName, string pDescription, string pActions) {
        _id = pId;
        _name = pName;
        _description = pDescription;
        _actions = pActions;
    }
}
