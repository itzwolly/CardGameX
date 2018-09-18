using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCollectionBehaviour : MonoBehaviour {
    [SerializeField] private Text _id;
    [SerializeField] private Text _name;
    [SerializeField] private Text _description;
    [SerializeField] private Text _actions;

    private CardData _cardData;
    private Button _btnCard;

    public CardData CardData {
        get { return _cardData; }
    }

    private void Start() {
        _btnCard = GetComponent<Button>();
        _btnCard.onClick.AddListener(delegate { AddCardToDeck(_cardData); });
    }

    public void SetCardData(CardData pCardData) {
        _cardData = pCardData;
        SetCardText(_cardData);
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
    }
}
