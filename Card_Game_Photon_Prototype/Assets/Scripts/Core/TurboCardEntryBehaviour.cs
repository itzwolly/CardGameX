using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboCardEntryBehaviour : MonoBehaviour {
    private CardData _cardData;
    private OnPointerClickHandler _onClickCardEntry;

    public void SetCardData(CardData pCardData) {
        _cardData = pCardData;
        _onClickCardEntry = GetComponent<OnPointerClickHandler>();
        _onClickCardEntry.OnLeftClick.AddListener(delegate { RemoveCardFromTurboDeck(_cardData); });
    }

    private void RemoveCardFromTurboDeck(CardData pCardData) {
        Debug.Log("Clicked on remove from turbo");

        if (pCardData == null) {
            Debug.Log("Card data equals null");
            return;
        }

        Card card = DeckHandler.Instance.DeckToEdit.GetTurboCard(pCardData);
        DeckHandler.Instance.DeckToEdit.RemoveTurboCard(card);
    }
}
