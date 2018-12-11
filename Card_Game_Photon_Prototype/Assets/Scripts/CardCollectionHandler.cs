using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class CardCollectionHandler : MonoBehaviour {
    [SerializeField] private GameObject _cardCollectionHUD;
    [SerializeField] private GameObject _cardCollectionPrefab;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private CardCollectionList _cardCollectionList;

    private const string CARD_COLLECTION_URL_USING_JSON = "http://card-game.gearhostpreview.com/cardcollection.json";

    private int _pageIndex;
    private int _cardsInPageAmount;
    private bool _hasFinishedLoadingCards;

    private Coroutine _crGetCardCollection;
    private GameObject _cardCollectionParent;

    public Coroutine CRGetCardCollection {
        get { return _crGetCardCollection; }
    }

    public int PageIndex {
        get { return _pageIndex; }
        set {
            _pageIndex = value;

            if (_pageIndex < 0) {
                _pageIndex = 0;
            }

            if (_crGetCardCollection != null) {
                StopCoroutine(_crGetCardCollection);
                _crGetCardCollection = null;
            }

            _cardsInPageAmount = 0;
            _hasFinishedLoadingCards = false;
            CreateCardCollectionParent();

            _crGetCardCollection = StartCoroutine(GetCardCollectionFromJSON());
        }
    }
    public int CardsInPageAmount {
        get { return _cardsInPageAmount; }
    }
    public bool HasFinishedLoadingCards {
        get { return _hasFinishedLoadingCards; }
    }
    public GameObject CardCollectionHUD {
        get { return _cardCollectionHUD; }
    }

    private void Start () {
        _pageIndex = 0;
        _cardsInPageAmount = 0;
        _hasFinishedLoadingCards = false;
        CreateCardCollectionParent();

        _crGetCardCollection = StartCoroutine(GetCardCollectionFromJSON());
    }

    private void CreateCardCollectionParent() {
        if (_cardCollectionParent != null) {
            Destroy(_cardCollectionParent);
        }
        _cardCollectionParent = Instantiate(_cardCollectionPrefab);
        _cardCollectionParent.transform.SetParent(_cardCollectionHUD.transform, false);
    }

    private GameObject CreateCardContainer(CardData pCardData) {
        GameObject card = Instantiate(_cardPrefab);
        _cardCollectionParent.transform.SetParent(_cardCollectionHUD.transform, false);
        card.transform.SetParent(_cardCollectionParent.transform, false);

        RectTransform cardRectTransform = card.GetComponent<RectTransform>();
        Rect cardRect = cardRectTransform.rect;

        Canvas cardChildCanvas = card.GetComponentInChildren<Canvas>();
        RectTransform cardChildRectTransform = cardChildCanvas.GetComponent<RectTransform>();
        Vector2 _center = new Vector2(0.5f, 0.5f);
        cardChildRectTransform.anchorMin = _center;
        cardChildRectTransform.anchorMax = _center;
        cardChildRectTransform.pivot = _center;
        cardChildRectTransform.anchoredPosition = Vector2.zero;
        cardChildRectTransform.sizeDelta = new Vector2(cardRect.width, cardRect.height);

        CardBehaviour ccb = card.GetComponent<CardBehaviour>();
        ccb.SetCardData(pCardData);

        return card;
    }
    
    private IEnumerator GetCardCollectionFromJSON() {
        using (WWW hs_get = new WWW(CARD_COLLECTION_URL_USING_JSON)) {
            yield return hs_get;

            if (hs_get.error != null) {
                print("There was an error getting the card collection: " + hs_get.error);
            } else {
                CardInfoList infoList = CardInfoList.CreateFromJSON(hs_get.text);

                IEnumerable<CardInfo> result = infoList.Cards.Skip(Config.MAX_CARDS_PER_PAGE * _pageIndex).Take(Config.MAX_CARDS_PER_PAGE);

                for (int i = 0; i < result.Count(); i++) {
                    CardInfo info = result.ElementAt(i);
                    CardData.CardType type = CardData.ValidateType(info.type);
                    CardData data = null;

                    switch (type) {
                        case CardData.CardType.Spell:
                            data = new SpellCardData(info.id, info.name, (info.description == null) ? "" : info.description, info.regcost, info.turbocost);
                            break;
                        case CardData.CardType.Monster:
                            data = new MonsterCardData(info.id, info.name, (info.description == null) ? "" : info.description, info.regcost, info.turbocost, info.attack, info.health, PhotonNetwork.player.ID);
                            break;
                        case CardData.CardType.None:
                        default:
                            break;
                    }
                    
                    CreateCardContainer(data);
                    _cardsInPageAmount++;
                }
                _hasFinishedLoadingCards = true;
            }
        }
    }
}
