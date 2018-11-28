using UnityEngine;

public class Card {
    public CardData Data;
    public bool IsTurbo;

    public Card(CardData pCardData, bool pIsTurbo) {
        Data = pCardData;
        IsTurbo = pIsTurbo;
    }

    public bool IsMonster() {
        bool isMonster = Data is MonsterCardData;
        //bool isMonster = typeof(MonsterCardData).IsAssignableFrom(_data.GetType());
        Debug.Log("IsMonster equals: " + isMonster + " for: " + Data.Id + " | " + Data.Name + " is turbo: " + IsTurbo);

        return isMonster;
    }
}
