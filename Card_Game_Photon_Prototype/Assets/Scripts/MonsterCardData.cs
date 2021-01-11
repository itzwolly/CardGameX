using System;

public class MonsterCardData : CardData, IInteractable {

    private int _attack;
    private int _health;

    public int BoardIndex { get; set; }
    public readonly int OwnerId;

    public int Attack {
        get { return _attack; }
        set { _attack = value; }
    }

    public int Health {
        get { return _health; }
        set {
            _health = value;

            if (_health < 0) {
                _health = 0;
            }
        }
    }

    public MonsterCardData(int pId,
                           string pName,
                           string pDescription,
                           int pRegCost,
                           int pTurboCost,
                           int pAttack,
                           int pHealth,
                           int pOwner) : base(pId, pName, pDescription, pRegCost, pTurboCost) 
    {
        _attack = pAttack;
        _health = pHealth;
        OwnerId = pOwner;
    }

    public int GetId() {
        return Id;
    }

    public int GetBoardIndex() {
        return BoardIndex;
    }

    public int GetOwnerId() {
        return OwnerId;
    }
}
