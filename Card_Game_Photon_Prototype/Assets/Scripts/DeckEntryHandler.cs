using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckEntryHandler : MonoBehaviour {
    private Deck _deck;
    private OnClickCardCollection _onClick;
    private OnPointerHoverHandler _onHover;

    private void Start() {
        _onClick = GameObject.FindGameObjectWithTag("OnClickHandler").GetComponent<OnClickCardCollection>();
        _onHover = GetComponent<OnPointerHoverHandler>();
    }

    public void SetDeckData(DeckData pData) {
        _deck = new Deck(pData);

        LoadCards(_deck);
    }

	public void DeleteEntry() {
        _onClick.OnClickRemoveDeck(gameObject, _deck);
    }

    public void DisplayDeck() {
        GetComponentInParent<Canvas>().gameObject.SetActive(false);
        CardCollectionList.Instance.LoadCardsInDeck(_deck);
    }

    private void LoadCards(Deck pDeck) {
        StartCoroutine(WebServer.GetCardsUsingDeckCode(pDeck.Data.DeckCode, pDeck.Data.TurboCode, (regCardsList, turboCardsList) => {
            if (regCardsList != null) {
                foreach (CardData data in regCardsList) {
                    Card card = new Card(data, false);
                    pDeck.Cards.Add(card);
                }
            }
            if (turboCardsList != null) {
                foreach (CardData data in turboCardsList) {
                    Card card = new Card(data, true);
                    pDeck.Turbo.Add(card);
                }
            }
        }));
    }

    private void Update() {
        if (_onHover.Hover) {
            if (Input.GetKeyUp(KeyCode.Space)) {
                if (_deck != null) {
                    DeckHandler.Instance.SelectedDeck = _deck;
                }
            }
        }
    }
}
