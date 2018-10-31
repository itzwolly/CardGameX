using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardCollectionList : MonoBehaviour {
    private static CardCollectionList _cardCollectionList;

    public static CardCollectionList Instance {
        get {
            if (!_cardCollectionList) {
                _cardCollectionList = FindObjectOfType(typeof(CardCollectionList)) as CardCollectionList;

                if (!_cardCollectionList) {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                } else {
                    _cardCollectionList.Init();
                }
            }
            return _cardCollectionList;
        }
    }

    [SerializeField] private GameObject _cardCollectionHUD;
    [SerializeField] private GameObject _cardEntryContainer;
    [SerializeField] private GameObject _turboCardEntryContainer;
    [SerializeField] private GameObject _cardEntryPrefab;

    private Dictionary<int, GameObject> _regContainers;
    private Dictionary<int, GameObject> _turboContainers;
    private List<Card> _localRegDeck;
    private List<Card> _localTurboDeck;
    private OnInputFieldHandler _inputFieldHandler;

    public List<Card> LocalRegDeck {
        get { return _localRegDeck; }
        set { _localRegDeck = value; }
    }
    public List<Card> LocalTurboDeck {
        get { return _localTurboDeck; }
        set { _localTurboDeck = value; }
    }

    private void Init() {
        if (_regContainers == null && _turboContainers == null) {
            _regContainers = new Dictionary<int, GameObject>();
            _turboContainers = new Dictionary<int, GameObject>();

            _inputFieldHandler = GameObject.FindGameObjectWithTag("OnInputFieldHandler").GetComponent<OnInputFieldHandler>();
        }
    }

    public void ResetDecks() {
        if (DeckHandler.Instance.DeckToEdit != null) {
            DeckHandler.Instance.DeckToEdit.Cards = _localRegDeck;
            DeckHandler.Instance.DeckToEdit.Turbo = _localTurboDeck;
        }
    }

    public void LoadCardsInDeck(Deck pDeck) {
        DeckHandler.Instance.DeckToEdit = pDeck;
        _localRegDeck = new List<Card>(pDeck.Cards);
        _localTurboDeck = new List<Card>(pDeck.Turbo);

        _inputFieldHandler.SetDeckData(pDeck.Data.Name);

        ClearContainers();
        _cardCollectionHUD.SetActive(true);

        InitializeCardEntries(pDeck.Cards, false);
        InitializeCardEntries(pDeck.Turbo, true);
    }

    private void InitializeCardEntries(List<Card> pCards, bool pIsTurbo) {
        var duplicateList = pCards.GroupBy(x => x.Data.Name).Select(x => new {
            Count = x.Count(),
            Id = x.First().Data.Id,
        });

        var distinctList = pCards.GroupBy(x => x.Data.Name).Select(o => o.First());
        for (int i = 0; i < distinctList.Count(); i++) {
            CardData data = distinctList.ElementAt(i).Data;
            int duplicates = duplicateList.ElementAt(i).Count;

            AddCardEntryOnLoad(data, duplicates, pIsTurbo);
        }
    }

    private void ClearContainers() {
        foreach (GameObject go in _regContainers.Values) {
            Destroy(go);
        }
        foreach (GameObject go in _turboContainers.Values) {
            Destroy(go);
        }
        _regContainers.Clear();
        _turboContainers.Clear();
    }

    private void AddCardEntryOnLoad(CardData pCardData, int pDuplicateAmount, bool pIsTurbo = false) {
        if (pCardData == null) {
            Debug.Log("The card you are trying to add to the visual list is null.");
            return;
        }

        GameObject entry = Instantiate(_cardEntryPrefab);

        if (!pIsTurbo) {
            entry.transform.SetParent(_cardEntryContainer.transform, false);
            RegCardEntryBehaviour behaviour = entry.AddComponent<RegCardEntryBehaviour>();
            behaviour.SetCardData(pCardData);

            _regContainers.Add(pCardData.Id, entry);
            _regContainers[pCardData.Id].GetComponentInChildren<Text>().text = pCardData.Name + " " + pDuplicateAmount + "x";
        } else {
            entry.transform.SetParent(_turboCardEntryContainer.transform, false);
            TurboCardEntryBehaviour behaviour = entry.AddComponent<TurboCardEntryBehaviour>();
            behaviour.SetCardData(pCardData);

            _turboContainers.Add(pCardData.Id, entry);
            _turboContainers[pCardData.Id].GetComponentInChildren<Text>().text = pCardData.Name + " " + pDuplicateAmount + "x";
        }
    }

    public void AddCardEntry(CardData pCardData, int pDuplicateAmount, bool pIsTurbo = false) {
        if (pCardData == null) {
            Debug.Log("The card you are trying to add to the visual list is null.");
            return;
        }

        GameObject entry;

        if (pDuplicateAmount == 0) {
            entry = Instantiate(_cardEntryPrefab);

            if (!pIsTurbo) {
                entry.transform.SetParent(_cardEntryContainer.transform, false);
                RegCardEntryBehaviour behaviour = entry.AddComponent<RegCardEntryBehaviour>();
                behaviour.SetCardData(pCardData);

                _regContainers.Add(pCardData.Id, entry);
            } else {
                Debug.Log("Adding turbo card..");
                entry.transform.SetParent(_turboCardEntryContainer.transform, false);
                TurboCardEntryBehaviour behaviour = entry.AddComponent<TurboCardEntryBehaviour>();
                behaviour.SetCardData(pCardData);

                _turboContainers.Add(pCardData.Id, entry);
            }
        } else {
            if (!pIsTurbo) {
                entry = _regContainers[pCardData.Id];
            } else {
                entry = _turboContainers[pCardData.Id];
            }
        }

        

        int localDuplicateAmount = pDuplicateAmount + 1;
        entry.GetComponentInChildren<Text>().text = pCardData.Name + " " + localDuplicateAmount + "x";
    }

    public void RemoveCardEntry(CardData pCardData, int pDuplicateAmount, bool pIsTurbo = false) {
        if (pCardData == null) {
            Debug.Log("The card you are trying to remove from the visual list is null.");
            return;
        }

        GameObject entry;

        if (!pIsTurbo) {
            entry = _regContainers[pCardData.Id];
        } else {
            entry = _turboContainers[pCardData.Id];
        }
        
        if (pDuplicateAmount == 0) {
            if (!pIsTurbo) {
                _regContainers.Remove(pCardData.Id);
            } else {
                _turboContainers.Remove(pCardData.Id);
            }

            Destroy(entry);
            return;
        }

        entry.GetComponentInChildren<Text>().text = pCardData.Name + " " + pDuplicateAmount + "x";
    }
}
