﻿using System;
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

    private Deck _deckToEdit;
    private Deck _selectedDeck;

    public Deck DeckToEdit {
        get { return _deckToEdit; }
        set { _deckToEdit = value; }
    }
    public Deck SelectedDeck {
        get { return _selectedDeck; }
        set { _selectedDeck = value; }
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
