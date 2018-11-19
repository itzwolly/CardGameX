using UnityEngine;

public class Card {
    private CardData _data;

    public CardData Data {
        get { return _data; }
    }

    public Card(CardData pCardData) {
        _data = pCardData;
    }

    public bool IsMonster() {
        bool isMonster = _data is MonsterCardData;
        //bool isMonster = typeof(MonsterCardData).IsAssignableFrom(_data.GetType());
        Debug.Log("IsMonster equals: " + isMonster + " for: " + Data.Id + " | " + Data.Name);

        return isMonster;
    }
}
