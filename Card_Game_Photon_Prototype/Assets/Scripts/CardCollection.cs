using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCollection : MonoBehaviour {
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _cardCollectionPrefab;
    [SerializeField] private GameObject _cardPrefab;

    public const int MAX_CARDS_PER_PAGE = 21;
    private const string CARD_COLLECTION_URL = "http://card-game.gearhostpreview.com/cardcollection.php";
    
    private int _pageIndex;
    private int _cardsInPageAmount;
    private bool _hasFinishedLoadingCards;

    private Coroutine _crGetCardCollection;
    private GameObject _cardCollectionParent;

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
            _crGetCardCollection = StartCoroutine(GetCardCollection());
        }
    }
    public int CardsInPageAmount {
        get { return _cardsInPageAmount; }
    }
    public bool HasFinishedLoadingCards {
        get { return _hasFinishedLoadingCards; }
    }

    private void Start () {
        _pageIndex = 0;
        _cardsInPageAmount = 0;
        _hasFinishedLoadingCards = false;
        CreateCardCollectionParent();
        _crGetCardCollection = StartCoroutine(GetCardCollection());
    }

    private void CreateCardCollectionParent() {
        if (_cardCollectionParent != null) {
            Destroy(_cardCollectionParent);
        }
        _cardCollectionParent = Instantiate(_cardCollectionPrefab);
        _cardCollectionParent.transform.SetParent(_hud.transform, false);
    }

    private GameObject CreateCardContainer(string[] pCardData) {
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

        GameObject cardChildContainer = cardChildCanvas.gameObject;
        foreach (Transform child in cardChildContainer.transform) {
            if (child.tag == "CardContainerData") {
                for (int i = 0; i < pCardData.Length; i++) {
                    child.GetChild(i).GetComponent<Text>().text = pCardData[i];
                }
            }
        }
        
        return card;
    }

    private IEnumerator GetCardCollection() {
        WWWForm form = new WWWForm();
        form.AddField("pageindex", _pageIndex);
        form.AddField("maxcardsperpage", MAX_CARDS_PER_PAGE);

        Dictionary<string, string> headers = form.headers;
        byte[] postData = form.data;

        using (WWW hs_get = new WWW(CARD_COLLECTION_URL, postData)) {
            yield return hs_get;

            if (hs_get.error != null) {
                print("There was an error getting the card collection: " + hs_get.error);
            } else {
                string[] rows = hs_get.text.Split('\n');
                // the last entry in the row is the '\n', not interested in that only interested in the split lines on '\t'.
                for (int i = 0; i < rows.Length - 1; i++) { 
                    string[] cols = rows[i].Split('\t');
                    CreateCardContainer(cols);
                    _cardsInPageAmount++;
                }
                _hasFinishedLoadingCards = true;
            }
        }
    }
}
