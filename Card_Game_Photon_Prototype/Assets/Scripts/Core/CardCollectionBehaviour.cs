
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardCollectionBehaviour : MonoBehaviour {
    [SerializeField] private Text _id;
    [SerializeField] private Text _name;
    [SerializeField] private Text _description;
    [SerializeField] private Text _actions;

    private CardData _cardData;
    private OnCollectionCardClick _onClickCard;
    private CardCollectionList _collectionList;

    public CardData CardData {
        get { return _cardData; }
    }

    private void Start() {
        _onClickCard = GetComponent<OnCollectionCardClick>();
        _onClickCard.OnLeftClick.AddListener(delegate { AddCardToDeck(_cardData); });
        _onClickCard.OnRightClick.AddListener(delegate { RemoveCardFromDeck(_cardData); });
    }

    public void SetCardData(CardData pCardData) {
        _cardData = pCardData;
        SetCardText(_cardData);
    }

    private void SetCardText(CardData pCardData) {
        _id.text = pCardData.Id.ToString();
        _name.text = pCardData.Name;
        _description.text = pCardData.Description;
        _actions.text = pCardData.ActionsToString;
    }

    private void AddCardToDeck(CardData pCardData) {
        if (pCardData == null) {
            Debug.Log("Card data equals null");
            return;
        }

        Card card = new Card(pCardData);
        StartCoroutine(WebServer.AddCardToDeck("user_1", card.Data.Id, "Dummy_deck_name"));
        //DeckHandler.Instance.ActiveDeck.AddCard(card);
    }

    private void RemoveCardFromDeck(CardData pCardData) {
        if (pCardData == null) {
            Debug.Log("Card data equals null");
            return;
        }

        Card card = DeckHandler.Instance.ActiveDeck.GetCard(pCardData);
        DeckHandler.Instance.ActiveDeck.RemoveCard(card);
    }
}
