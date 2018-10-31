using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
    private static Hand _instance;
    public static Hand Instance {
        get {
            if (!_instance) {
                _instance = FindObjectOfType(typeof(Hand)) as Hand;

                if (!_instance) {
                    Debug.Log("There should be an active object with the component Hand");
                } else {
                    //_instance.Init();
                }
            }
            return _instance;
        }
    }

    public GameObject DisplayCard(int pCardId, int pHandSize) {
        HandManager manager = PhotonNetwork.player.TagObject as HandManager;
        GameObject gameCard = manager.DisplayPlayerCardDrawn(pHandSize);
        gameCard.GetComponent<CardGameBehaviour>().CardId = pCardId;

        return gameCard;
    }
}
