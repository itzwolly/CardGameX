using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : PunTurnManager {
    private static TurnManager _instance;

    public static TurnManager Instance
    {
        get
        {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<TurnManager>();

                if (_instance == null) {
                    GameObject container = new GameObject("Turn Manager");
                    _instance = container.AddComponent<TurnManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        gameObject.AddComponent<PunTurnManager>();
    }
}
