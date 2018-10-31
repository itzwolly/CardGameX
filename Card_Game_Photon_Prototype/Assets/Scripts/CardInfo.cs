using UnityEngine;

[System.Serializable]
public class CardInfo : System.Object {
    public int id;
    public string name;
    public string description;
    public string type;
    public int attack;
    public int health;
    public int regcost;
    public int turbocost;

    public static CardInfo CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<CardInfo>(jsonString);
    }
}