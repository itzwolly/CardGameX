using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickCardCollection : MonoBehaviour {
    [SerializeField] private CardCollectionHandler _cardCollection;
    [SerializeField] private GameObject _btnNextPage;
    [SerializeField] private GameObject _btnPrevPage;

    private void Start() {
        _btnNextPage.SetActive(true);
        _btnPrevPage.SetActive(false);
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
