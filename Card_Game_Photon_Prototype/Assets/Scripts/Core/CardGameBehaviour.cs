using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardGameBehaviour : MonoBehaviour {
    [SerializeField] private int _cardId;

    public int CardId {
        get { return _cardId; }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (transform.tag == "Interactable") {
                if (TurnManager.Instance.IsActivePlayer) { // Only allowed to play cards if it's your turn.
                    Events.RaiseCardPlayedEvent(_cardId, transform.GetSiblingIndex());
                } else {
                    TurnManager.Instance.TurnManagerListener.OnNotYourTurn();
                }
            }
        }
    }
}
