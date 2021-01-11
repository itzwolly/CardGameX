using UnityEngine;

[System.Serializable]
public class DeckInfoList : System.Object {
    public DeckInfo[] Decks;

    public static DeckInfoList CreateFromJSON(string pJsonString) {
        return JsonUtility.FromJson<DeckInfoList>(pJsonString);
    }
}