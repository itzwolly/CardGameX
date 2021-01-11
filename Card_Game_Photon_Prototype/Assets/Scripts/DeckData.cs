using System;

public class DeckData {
    private int _id;
    private string _name;
    private string _deckCode;
    private string _turboCode;

    public int Id {
        get { return _id; }
        set { _id = value; }
    }
    public string Name {
        get { return _name; }
        set { _name = value; }
    }
    public string DeckCode {
        get { return _deckCode; }
        set { _deckCode = value; }
    }
    public string TurboCode {
        get { return _turboCode; }
        set { _turboCode = value; }
    }

    public DeckData(int pId, string pName, string pDeckCode, string pTurboCode = "") {
        _id = pId;
        _name = pName;
        _deckCode = pDeckCode;
        _turboCode = pTurboCode;
    }
}
