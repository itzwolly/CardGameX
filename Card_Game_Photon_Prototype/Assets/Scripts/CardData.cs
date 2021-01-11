using System;

public class CardData {
    private int _id;
    private string _name;
    private string _description;
    private int _regCost;
    private int _turboCost;

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
    public int RegCost {
        get { return _regCost; }
        set { _regCost = value; }
    }
    public int TurboCost {
        get { return _turboCost; }
        set { _turboCost = value; }
    }

    public enum CardType {
        None,
        Spell,
        Monster
    }

    public CardData(int pId,
                    string pName,
                    string pDescription,
                    int pRegCost,
                    int pTurboCost)
    {
        _id = pId;
        _name = pName;
        _description = pDescription;
        _regCost = pRegCost;
        _turboCost = pTurboCost;
    }

    public static CardType ValidateType(string pCardType) {
        CardType type = (CardType) Enum.Parse(typeof(CardType), pCardType);
        if (Enum.IsDefined(typeof(CardType), pCardType)) {
            return type;
        }
        return CardType.None;
    }
}
