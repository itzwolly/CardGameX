using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckHandler : MonoBehaviour {

    private static DeckHandler _instance;

    public static DeckHandler Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<DeckHandler>();

                if (_instance == null) {
                    GameObject container = new GameObject("Deck Manager");
                    _instance = container.AddComponent<DeckHandler>();
                }
            }
            return _instance;
        }
    }

    private Deck _activeDeck;

    public Deck ActiveDeck {
        get { return _activeDeck; }
        set { _activeDeck = value; }
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        _activeDeck = new Deck(0, "DUMMY_NAME"); // Temporary
    }
}
