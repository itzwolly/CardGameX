using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegCardEntryBehaviour : MonoBehaviour {
    private CardData _cardData;
    private OnPointerClickHandler _onClickCardEntry;

    public void SetCardData(CardData pCardData) {
        _cardData = pCardData;
        _onClickCardEntry = GetComponent<OnPointerClickHandler>();
        _onClickCardEntry.OnLeftClick.AddListener(delegate { RemoveCardFromRegDeck(_cardData); });
    }

    private void RemoveCardFromRegDeck(CardData pCardData) {
        if (pCardData == null) {
            Debug.Log("Card data equals null");
            return;
        }

        Card card = DeckHandler.Instance.DeckToEdit.GetRegCard(pCardData);
        DeckHandler.Instance.DeckToEdit.RemoveRegCard(card);
    }
}
