using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class CardCollectionHandler : MonoBehaviour {
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _cardCollectionPrefab;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private CardCollectionList _cardCollectionList;
    
    private const string CARD_COLLECTION_URL_USING_DB = "http://card-game.gearhostpreview.com/cardcollection.php";
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

            //_crGetCardCollection = StartCoroutine(GetCardCollectionFromDB());
            _crGetCardCollection = StartCoroutine(GetCardCollectionFromJSON());
        }
    }
    public int CardsInPageAmount {
        get { return _cardsInPageAmount; }
    }
    public bool HasFinishedLoadingCards {
        get { return _hasFinishedLoadingCards; }
    }
    public GameObject HUD {
        get { return _hud; }
    }

    private void Start () {
        _pageIndex = 0;
        _cardsInPageAmount = 0;
        _hasFinishedLoadingCards = false;
        CreateCardCollectionParent();

        //_crGetCardCollection = StartCoroutine(GetCardCollectionFromDB());
        _crGetCardCollection = StartCoroutine(GetCardCollectionFromJSON());
    }

    private void CreateCardCollectionParent() {
        if (_cardCollectionParent != null) {
            Destroy(_cardCollectionParent);
        }
        _cardCollectionParent = Instantiate(_cardCollectionPrefab);
        _cardCollectionParent.transform.SetParent(_hud.transform, false);
    }

    private GameObject CreateCardContainer(CardData pCardData) {
        GameObject card = Instantiate(_cardPrefab);
        _cardCollectionParent.transform.SetParent(_hud.transform, false);
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

        CardCollectionBehaviour ccb = card.GetComponent<CardCollectionBehaviour>();
        ccb.SetCardData(pCardData);

        return card;
    }

    private IEnumerator GetCardCollectionFromDB() {
        WWWForm form = new WWWForm();
        form.AddField("pageindex", _pageIndex);
        form.AddField("maxcardsperpage", Config.MAX_CARDS_PER_PAGE);

        Dictionary<string, string> headers = form.headers;
        byte[] postData = form.data;

        using (WWW hs_get = new WWW(CARD_COLLECTION_URL_USING_DB, postData)) {
            yield return hs_get;

            if (hs_get.error != null) {
                print("There was an error getting the card collection: " + hs_get.error);
            } else {
                string[] rows = hs_get.text.Split('\n');
                
                // using rows.length - 1 because:
                // the last entry in the row is the '\n', not interested in that only interested in the lines split on '\t'.
                for (int i = 0; i < rows.Length - 1; i++) { 
                    string[] cols = rows[i].Split('\t');
                    CardData data = new CardData(Convert.ToInt32(cols[0]), cols[1], cols[2], cols[3]);

                    CreateCardContainer(data);
                    _cardsInPageAmount++;
                }
                _hasFinishedLoadingCards = true;
            }
        }
    }

    private IEnumerator GetCardCollectionFromJSON() {
        using (WWW hs_get = new WWW(CARD_COLLECTION_URL_USING_JSON)) {
            yield return hs_get;

            if (hs_get.error != null) {
                print("There was an error getting the card collection: " + hs_get.error);
            } else {
                CardInfoList infoArray = CardInfoList.CreateFromJSON(hs_get.text);
                IEnumerable<CardInfo> result = infoArray.Cards.Skip(Config.MAX_CARDS_PER_PAGE * _pageIndex).Take(Config.MAX_CARDS_PER_PAGE);

                for (int i = 0; i < result.Count(); i++) {
                    CardInfo info = result.ElementAt(i);
                    CardData data = new CardData(info.Id, (info.Name == null) ? "" : info.Name, (info.Description == null) ? "" : info.Description, (info.Actions == null) ? "" : info.Actions);

                    CreateCardContainer(data);
                    _cardsInPageAmount++;
                }
                _hasFinishedLoadingCards = true;
            }
        }
    }
}
