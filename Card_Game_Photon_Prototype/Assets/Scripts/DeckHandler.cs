using System;
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

        if (_activeDeck == null) {
            _activeDeck = new Deck(0, "DUMMY_NAME"); // Temporary
            InitializeDeck("user_1");
        }
    }

    private void InitializeDeck(string pUserName) {
        StartCoroutine(WebServer.GetDeckFromDB(pUserName, (cardData) => {
            for (int i = 0; i < cardData.Length - 1; i++) {
                string[] cols = cardData[i].Split('\t');
                CardData data = new CardData(Convert.ToInt32(cols[0]), cols[1], cols[2], cols[2]);
                Card card = new Card(data);
                _activeDeck.AddCard(card);
            }
        }));
    }
}
