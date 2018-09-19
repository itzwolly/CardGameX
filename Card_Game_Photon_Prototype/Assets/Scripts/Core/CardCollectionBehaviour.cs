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
    private Transform _collectionListParent;

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

    public void SetCollectionListData(CardCollectionList pCollectionList, Transform pParent) {
        _collectionList = pCollectionList;
        _collectionListParent = pParent;
    }

    private void SetCardText(CardData pCardData) {
        _id.text = pCardData.Id.ToString();
        _name.text = pCardData.Name;
        _description.text = pCardData.Description;
        _actions.text = pCardData.Actions;
    }

    private void AddCardToDeck(CardData pCardData) {
        Card card = new Card(pCardData);
        DeckHandler.Instance.ActiveDeck.AddCard(card);
        _collectionList.AddCardEntry(card, _collectionListParent);
    }

    private void RemoveCardFromDeck(CardData pCardData) {
        Card card = DeckHandler.Instance.ActiveDeck.GetCard(pCardData);
        DeckHandler.Instance.ActiveDeck.RemoveCard(card);
        _collectionList.RemoveCardEntry(card);
    }
}
