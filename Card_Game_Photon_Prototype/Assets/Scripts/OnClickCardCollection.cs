using System.Linq;
using UnityEngine;

public class OnClickCardCollection : MonoBehaviour {
    [SerializeField] private CardCollectionHandler _cardCollection;
    [SerializeField] private DeckCollectionHandler _deckCollection;
    [SerializeField] private GameObject _panelSaveChangesToDeck;
    [SerializeField] private GameObject _panelConfirmDeckRemoval;
    [SerializeField] private GameObject _btnNextPage;
    [SerializeField] private GameObject _btnPrevPage;

    private OnInputFieldHandler _inputFieldHandler;
    private GameObject _deckGameObjectToRemove;
    private Deck _deckToRemove;

    private void Start() {
        _btnNextPage.SetActive(true);
        _btnPrevPage.SetActive(false);

        _inputFieldHandler = GameObject.FindGameObjectWithTag("OnInputFieldHandler").GetComponent<OnInputFieldHandler>();
    }

    public void OnClickNextPage() {
        if (_cardCollection.HasFinishedLoadingCards) {
            // Also GET's the new list of cards based on the index
            _cardCollection.PageIndex++;

            if (_cardCollection.PageIndex > 0) {
                _btnPrevPage.SetActive(true);
            }
        }
    }

    public void OnClickPreviousPage() {
        if (_cardCollection.HasFinishedLoadingCards) {
            // Also GET's the new list of cards based on the index
            _cardCollection.PageIndex--;

            if (_cardCollection.PageIndex == 0) {
                _btnPrevPage.SetActive(false);
            }
            if (!_btnNextPage.activeSelf) {
                _btnNextPage.SetActive(true);
            }
        }
    }

    public void OnClickPreviousScene() {
        LevelManager.Instance.LoadLevelASync(Config.MAIN_SCENE);
    }

    public void OnClickBackToDeckSelection() {
        if (DeckHandler.Instance.DeckToEdit.Cards.SequenceEqual(CardCollectionList.Instance.LocalRegDeck) 
            && DeckHandler.Instance.DeckToEdit.Turbo.SequenceEqual(CardCollectionList.Instance.LocalTurboDeck) 
            && _inputFieldHandler.DeckNameInput.text == DeckHandler.Instance.DeckToEdit.Data.Name) {
            Debug.Log("Sequence equal");

            ShowDeckCollection();
        } else {
            Debug.Log("Sequence not equal");
            // are you sure you want to save...
            _panelSaveChangesToDeck.SetActive(true);
        }
    }

    private void ShowDeckCollection() {
        DeckHandler.Instance.DeckToEdit = null;
        CardCollectionList.Instance.LocalRegDeck = null;

        _cardCollection.CardCollectionHUD.SetActive(false);
        _deckCollection.DeckCollectionHUD.SetActive(true);
    }

    public void OnClickYesToSaveDeck() {
        int id = DeckHandler.Instance.DeckToEdit.Data.Id;
        string prevDeckCode = DeckHandler.Instance.DeckToEdit.Data.DeckCode;
        string newDeckCode = DeckHandler.Instance.DeckToEdit.UpdateRegDeckCode();
        string turboDeckCode = DeckHandler.Instance.DeckToEdit.UpdateTurboCode();
        string newDeckName = _inputFieldHandler.DeckNameInput.text;

        StartCoroutine(WebServer.SaveCardsUsingDeckCode(id, newDeckName, PhotonNetwork.AuthValues.UserId, prevDeckCode, newDeckCode, turboDeckCode, (IsDone) => {
            if (IsDone) {
                _deckCollection.UpdateDeckName(DeckHandler.Instance.DeckToEdit.Data.Id, newDeckName);

                _panelSaveChangesToDeck.SetActive(false);
                ShowDeckCollection();
            }
        }));
    }

    public void OnClickNoToSaveDeck() {
        CardCollectionList.Instance.ResetDecks();
        _panelSaveChangesToDeck.SetActive(false);
        ShowDeckCollection();
    }

    public void OnClickCloseToSaveDeck() {
        _panelSaveChangesToDeck.SetActive(false);
    }

    public void OnClickRemoveDeck(GameObject pGameObject, Deck pDeck) {
        _deckGameObjectToRemove = pGameObject;
        _deckToRemove = pDeck;

        _panelConfirmDeckRemoval.SetActive(true);
    }

    public void OnClickYesToRemoveDeck() {
        Debug.Log("Clicked to remove deck");

        StartCoroutine(WebServer.RemoveDeck(PhotonNetwork.AuthValues.UserId, _deckToRemove.Data.Id));
        _deckCollection.RemoveContainer(_deckGameObjectToRemove);
        
        if (!_deckCollection.MaxContainerCountReached) {
            _deckCollection.AddButtonObj.SetActive(true);
        }

        _panelConfirmDeckRemoval.SetActive(false);
    }

    public void OnClickNoToRemoveDeck() {
        _deckGameObjectToRemove = null;
        _deckToRemove = null;

        _panelConfirmDeckRemoval.SetActive(false);
    }

    public void OnClickCloseToRemoveDeck() {
        _deckGameObjectToRemove = null;
        _deckToRemove = null;

        _panelConfirmDeckRemoval.SetActive(false);
    }

    public void OnClickAddDeck() {
        StartCoroutine(WebServer.AddDeck(PhotonNetwork.AuthValues.UserId, (deckData) => {
            _deckCollection.CreateDeckContainer(deckData);

            if (_deckCollection.MaxContainerCountReached) {
                _deckCollection.AddButtonObj.SetActive(false);
            }
        }));
    }

    private void Update() {
        if (_btnNextPage.activeSelf) {
            if (_cardCollection.HasFinishedLoadingCards) {
                if (_cardCollection.CardsInPageAmount < Config.MAX_CARDS_PER_PAGE) {
                    _btnNextPage.SetActive(false);
                }
            }
        }
    }
}
