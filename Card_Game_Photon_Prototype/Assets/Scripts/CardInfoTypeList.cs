using UnityEngine;

[System.Serializable]
public class CardInfoTypeList : System.Object {
    public CardInfoType CardTypes;

    public static CardInfoTypeList CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<CardInfoTypeList>(jsonString);
    }
}