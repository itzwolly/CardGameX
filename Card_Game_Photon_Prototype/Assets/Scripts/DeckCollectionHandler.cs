using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DeckCollectionHandler : MonoBehaviour {
    [SerializeField] private GameObject _deckCollectionHUD;
    [SerializeField] private GameObject _deckCollectionParent;
    [SerializeField] private GameObject _deckPrefab;
    [SerializeField] private GameObject _cardCollectionHUD;
    [SerializeField] private GameObject _addDeckButton;

    private Dictionary<int, GameObject> _deckContainers;

    private const string GET_PLAYER_DECKS_FROM_DB = "http://card-game.gearhostpreview.com/getplayerdecks.php";

    public GameObject DeckCollectionHUD {
        get { return _deckCollectionHUD; }
    }
    public GameObject AddButtonObj {
        get { return _addDeckButton; }
    }
    public int ContainerCount {
        get { return _deckContainers.Count; }
    }
    public bool MaxContainerCountReached {
        get { return ContainerCount == Config.MAX_DECK_SIZE; }
    }

    private void Start() {
        if (_deckContainers == null) {
            _deckContainers = new Dictionary<int, GameObject>();
        }

        CreateDeckCollectionParent();
        StartCoroutine(GetPlayerDecksFromDB("user_1"));
    }

    private void CreateDeckCollectionParent() {
        _deckCollectionParent = Instantiate(_deckCollectionParent);
        _deckCollectionParent.transform.SetParent(_deckCollectionHUD.transform, false);
    }

    public void CreateDeckContainer(DeckData pDeckData) {
        GameObject deck = Instantiate(_deckPrefab);
        _deckCollectionParent.transform.SetParent(_deckCollectionHUD.transform, false);
        deck.transform.SetParent(_deckCollectionParent.transform, false);

        deck.GetComponentInChildren<Text>().text = pDeckData.Name;
        deck.GetComponent<DeckEntryHandler>().SetDeckData(pDeckData);

        _deckContainers.Add(pDeckData.Id, deck);
    }

    public void UpdateDeckName(int pId, string pNewName) {
        DeckHandler.Instance.DeckToEdit.Data.Name = pNewName;

        GameObject deck = _deckContainers.FirstOrDefault(o => o.Key == pId).Value;
        if (deck != null) {
            deck.GetComponentInChildren<Text>().text = pNewName;
        }
    }

    public void RemoveContainer(GameObject pObj) {
        _deckContainers.Remove(_deckContainers.FirstOrDefault(o => o.Value == pObj).Key);
        Destroy(pObj);
    }

    private IEnumerator GetPlayerDecksFromDB(string pUsername) {
        WWWForm form = new WWWForm();
        form.AddField("username", pUsername);

        Dictionary<string, string> headers = form.headers;
        byte[] postData = form.data;

        using (WWW hs_get = new WWW(GET_PLAYER_DECKS_FROM_DB, postData)) {
            yield return hs_get;

            if (hs_get.error != null) {
                Debug.Log(hs_get.error);
            } else {
                //Debug.Log(hs_get.text);
                DeckInfoList infoArray = DeckInfoList.CreateFromJSON(hs_get.text);
                foreach (DeckInfo info in infoArray.Decks) {
                    //Debug.Log(info.DeckName + "," + info.DeckCode + "," + info.TurboCode);
                    DeckData data = new DeckData(info.Id, info.DeckName, info.DeckCode, info.TurboCode);
                    CreateDeckContainer(data);
                }

                if (MaxContainerCountReached) {
                    _addDeckButton.SetActive(false);
                }
            }
        }
    }
}
