using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardInfoType : System.Object {
    public CardInfo[] RegCards;
    public CardInfo[] TurboCards;

    public static CardInfoType CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<CardInfoType>(jsonString);
    }

    public List<CardData> GetCardInfo(CardInfo[] pCards) {
        if (pCards == null) {
            return null;
        }

        List<CardData> cards = new List<CardData>();
        foreach (CardInfo info in pCards) {
            CardData.CardType type = CardData.ValidateType(info.type);
            CardData data = null;

            switch (type) {
                case CardData.CardType.Spell:
                    data = new SpellCardData(info.id, info.name, (info.description == null) ? "" : info.description, info.regcost, info.turbocost);
                    cards.Add(data);
                    break;
                case CardData.CardType.Monster:
                    data = new MonsterCardData(info.id, info.name, (info.description == null) ? "" : info.description, info.regcost, info.turbocost, info.attack, info.health);
                    cards.Add(data);
                    break;
                case CardData.CardType.None:
                default:
                    break;
            }
        }
        return cards;
    }
}