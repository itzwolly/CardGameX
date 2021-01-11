using UnityEngine;

[System.Serializable]
public class DeckInfo : System.Object {
    public int Id;
    public string DeckName;
    public string DeckCode;
    public string TurboCode;

    public static DeckInfo CreateFromJSON(string pJsonString) {
        return JsonUtility.FromJson<DeckInfo>(pJsonString);
    }
}
