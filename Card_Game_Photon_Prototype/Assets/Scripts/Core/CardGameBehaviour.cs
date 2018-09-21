using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardGameBehaviour : MonoBehaviour {
    private Card _card;

    public Card Card {
        get { return _card; }
        set { _card = value; }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (transform.tag == "Interactable") {
                if (TurnManager.Instance.IsActivePlayer) { // Only allowed to play cards if it's your turn.
                    Events.RaiseCardPlayedEvent(_card.Data.Id, transform.GetSiblingIndex());
                } else {
                    TurnManager.Instance.TurnManagerListener.OnNotYourTurn(PhotonNetwork.player);
                }
            }
        }
    }
}
