using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardCollectionList : MonoBehaviour {
    [SerializeField] private GameObject _cardEntryPrefab;

    private Dictionary<int, GameObject> _containers;

    private void Awake() {
        _containers = new Dictionary<int, GameObject>();
    }

    public void AddCardEntry(Card pCard, Transform pParent) {
        if (pCard == null) {
            Debug.Log("The card you are trying to add to the visual list is null.");
            return;
        }

        int duplicates = DeckHandler.Instance.ActiveDeck.GetDuplicateCardCount(pCard);
        if (duplicates == 1) { // First instance of card X
            GameObject entry = Instantiate(_cardEntryPrefab);
            entry.transform.SetParent(pParent, false);
            _containers.Add(pCard.Data.Id, entry);
        }

        if (_containers.ContainsKey(pCard.Data.Id)) {
            _containers[pCard.Data.Id].GetComponentInChildren<Text>().text = pCard.Data.Name + " " + duplicates + "x";
        }
    }

    public void RemoveCardEntry(Card pCard) {
        if (pCard == null) {
            Debug.Log("The card you are trying to add to the visual list is null.");
            return;
        }

        int duplicates = DeckHandler.Instance.ActiveDeck.GetDuplicateCardCount(pCard);
        if (duplicates == 0) {
            Destroy(_containers[pCard.Data.Id]);
            _containers.Remove(pCard.Data.Id);
        }

        if (_containers.ContainsKey(pCard.Data.Id)) {
            _containers[pCard.Data.Id].GetComponentInChildren<Text>().text = pCard.Data.Name + " " + duplicates + "x";
        }
    }
}
