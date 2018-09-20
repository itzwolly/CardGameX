using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardGameBehaviour : MonoBehaviour {
    private Card _card;
    private bool _onBoard;

    public Card Card {
        get { return _card; }
        set { _card = value; }
    }

    private void Start() {
        _onBoard = false;
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (transform.tag == "Interactable") {
                if (TurnManager.Instance.IsActivePlayer) { // Only allowed to play cards if it's your turn.
                    Events.RaiseCardPlayedEvent(_card.Data.Id, transform.GetSiblingIndex());
                    _onBoard = true;
                } else {
                    TurnManager.Instance.TurnManagerListener.OnNotYourTurn();
                }
            }
        }
    }

    private void Update() {
        if (_onBoard) {
            _card.ExecuteOnStay();
        }
    }

    private void OnDisable() {
        if (_card != null) {
            if (_onBoard) {
                _card.ExecuteOnExit();
            }
            _onBoard = false;
        }
    }
}
