public class MonsterCardData : CardData {

    private int _attack;
    private int _health;

    public int Attack {
        get { return _attack; }
        set { _attack = value; }
    }

    public int Health {
        get { return _health; }
        set { _health = value; }
    }

    public MonsterCardData(int pId,
                           string pName,
                           string pDescription,
                           int pRegCost,
                           int pTurboCost,
                           int pAttack,
                           int pHealth) : base(pId, pName, pDescription, pRegCost, pTurboCost) 
    {
        _attack = pAttack;
        _health = pHealth;
    }
	
}
