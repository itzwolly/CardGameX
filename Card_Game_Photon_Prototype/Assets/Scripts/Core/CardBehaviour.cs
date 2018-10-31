using UnityEngine;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour {
    [SerializeField] private Text _name;
    [SerializeField] private Text _description;
    [SerializeField] private Text _regularCost;
    [SerializeField] private Text _turboCost;
    [SerializeField] private Text _attack;
    [SerializeField] private Text _health;

    private CardData _cardData;
    private OnPointerClickHandler _onClickCard;
    private CardCollectionList _collectionList;

    public CardData CardData {
        get { return _cardData; }
    }

    private void Start() {
        _onClickCard = GetComponent<OnPointerClickHandler>();
        _onClickCard.OnLeftClick.AddListener(delegate { AddCardToRegDeck(_cardData); });
        _onClickCard.OnRightClick.AddListener(delegate { AddCardToTurboDeck(_cardData); });
    }

    public void SetCardData(CardData pCardData) {
        _cardData = pCardData;
        SetCardText(_cardData);
    }

    private void SetCardText(CardData pCardData) {
        _name.text = pCardData.Name;
        _description.text = pCardData.Description;
        _regularCost.text = pCardData.RegCost.ToString();
        _turboCost.text = pCardData.TurboCost.ToString();

        if (pCardData is MonsterCardData) {
            MonsterCardData monsterData = (pCardData as MonsterCardData);
            _attack.text = monsterData.Attack.ToString();
            _health.text = monsterData.Health.ToString();
        }
    }

    private void AddCardToRegDeck(CardData pCardData) {
        if (pCardData == null) {
            Debug.Log("Card data equals null");
            return;
        }
        
        Card card = new Card(pCardData);
        DeckHandler.Instance.DeckToEdit.AddRegCard(card);
    }

    private void AddCardToTurboDeck(CardData pCardData) {
        if (pCardData == null) {
            Debug.Log("Card data equals null");
            return;
        }

        Card card = new Card(pCardData);
        DeckHandler.Instance.DeckToEdit.AddTurboCard(card);
    }
}
