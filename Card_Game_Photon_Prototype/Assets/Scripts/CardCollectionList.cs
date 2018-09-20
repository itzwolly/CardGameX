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

    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _cardEntryContainerPrefab;
    [SerializeField] private GameObject _cardEntryPrefab;

    private GameObject _cardEntryParent;
    private Dictionary<int, GameObject> _containers;

    private void Start() {
        if (_cardEntryParent == null) {
            _cardEntryParent = Instantiate(_cardEntryContainerPrefab);
            _cardEntryParent.transform.SetParent(_hud.transform, false);
        }
    }

    private void Init() {
        if (_containers == null) {
            _containers = new Dictionary<int, GameObject>();
        }
    }

    public void AddCardEntry(Card pCard, int pDuplicateAmount) {
        if (pCard == null) {
            Debug.Log("The card you are trying to add to the visual list is null.");
            return;
        }

        if (pDuplicateAmount == 0) {
            GameObject entry = Instantiate(_cardEntryPrefab);
            entry.transform.SetParent(_cardEntryParent.transform, false);
            _containers.Add(pCard.Data.Id, entry);
        }

        int localDuplicateAmount = pDuplicateAmount + 1;
        _containers[pCard.Data.Id].GetComponentInChildren<Text>().text = pCard.Data.Name + " " + localDuplicateAmount + "x";
    }

    public void RemoveCardEntry(Card pCard, int pDuplicateAmount) {
        if (pCard == null) {
            Debug.Log("The card you are trying to remove from the visual list is null.");
            return;
        }

        if (pDuplicateAmount == 0) {
            GameObject obj = _containers[pCard.Data.Id];
            _containers.Remove(pCard.Data.Id);
            Destroy(obj);
            return;
        }
        
        _containers[pCard.Data.Id].GetComponentInChildren<Text>().text = pCard.Data.Name + " " + pDuplicateAmount + "x";
    }
}
