using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour {
    [SerializeField] private int _cardId;

    public int CardId {
        get { return _cardId; }
    }

    private void OnMouseOver() {
        if (TurnManager.Instance.IsActivePlayer) { // Only allowed to play cards if it's your turn.
            if (Input.GetMouseButtonUp(0)) {
                if (transform.tag == "Interactable") {
                    Events.RaiseCardPlayedEvent(_cardId, transform.GetSiblingIndex());
                }
            }
        }
    }
}
