using UnityEngine;

[System.Serializable]
public class CardInfo : System.Object {
    public int Id;
    public string Name;
    public string Description;
    public string Actions;

    public static CardInfo CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<CardInfo>(jsonString);
    }
}