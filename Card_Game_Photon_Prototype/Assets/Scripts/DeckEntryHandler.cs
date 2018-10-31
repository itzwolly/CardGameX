using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckEntryHandler : MonoBehaviour {
    private OnClickCardCollection _onClick;
    private Deck _deck;

    private void Start() {
        _onClick = GameObject.FindGameObjectWithTag("OnClickHandler").GetComponent<OnClickCardCollection>();
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
                    Card card = new Card(data);
                    pDeck.Cards.Add(card);
                }
            }
            if (turboCardsList != null) {
                foreach (CardData data in turboCardsList) {
                    Card card = new Card(data);
                    pDeck.Turbo.Add(card);
                }
            }
        }));
    }
}
