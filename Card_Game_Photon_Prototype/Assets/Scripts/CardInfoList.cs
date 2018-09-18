using UnityEngine;

[System.Serializable]
public class CardInfoList : System.Object {
    public CardInfo[] Cards;

    public static CardInfoList CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<CardInfoList>(jsonString);
    }
}