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
    [SerializeField] private GameObject _cardEntryPrefab;
    
    private CardData _cardData;
    private OnCollectionCardClick _onClickCard;
    private GameObject _cardEntryContainer;

    public CardData CardData {
        get { return _cardData; }
    }
    public GameObject CardEntryContainer {
        get { return _cardEntryContainer; }
    }

    private void Start() {
        _onClickCard = GetComponent<OnCollectionCardClick>();
        _onClickCard.OnLeftClick.AddListener(delegate { AddCardToDeck(_cardData); });
        _onClickCard.OnRightClick.AddListener(delegate { RemoveCardFromDeck(_cardData); });
    }

    public void SetCardEntryContainer(GameObject pGameObject) {
        _cardEntryContainer = pGameObject;
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
        UpdateCardEntries();
    }

    private void RemoveCardFromDeck(CardData pCardData) {
        Card card = DeckHandler.Instance.ActiveDeck.GetCard(pCardData);
        DeckHandler.Instance.ActiveDeck.RemoveCard(card);
        UpdateCardEntries();
    }

    private void UpdateCardEntries() {
        DeleteChildren(_cardEntryContainer.transform);
        IEnumerable<Card> cards = DeckHandler.Instance.ActiveDeck.GetCards().GroupBy(x => x.Data.Id).Select(x => x.FirstOrDefault());

        for (int i = 0; i < cards.Count(); i++) {
            Card card = cards.ElementAt(i);
            int duplicates = DeckHandler.Instance.ActiveDeck.GetDuplicateCardCount(card);

            GameObject cardEntry = Instantiate(_cardEntryPrefab);
            cardEntry.transform.SetParent(_cardEntryContainer.transform, false);
            cardEntry.GetComponentInChildren<Text>().text = card.Data.Name + " " + duplicates + "x";
        }
    }

    private void DeleteChildren(Transform pTransform) {
        if (pTransform.childCount > 0) {
            foreach (Transform child in pTransform) {
                Destroy(child.gameObject);
            }
        }
    }


    private void SetText(Text pTarget, string pText) {
        pTarget.text = pText;
    }
}
