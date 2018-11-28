using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurboDeckBehaviour : MonoBehaviour {
    private static TurboDeckBehaviour _instance;

    public static TurboDeckBehaviour Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<TurboDeckBehaviour>();

                if (_instance == null) {
                    Debug.Log("No PlayerHandler in scene..");
                }
            }
            return _instance;
        }
    }

    [SerializeField] private GameObject _turboPlayerContainer;
    [SerializeField] private GameObject _turboPlayerCardPrefab;
    [SerializeField] private GameObject _turboEnemyContainer;
    [SerializeField] private GameObject _turboEnemyCardPrefab;
    [SerializeField] private Button  _btnHide;
    [SerializeField] private int _offset = 2;

    [HideInInspector] public bool DisplayCards = false;

    private void Start() {
        _btnHide.onClick.AddListener(delegate { ClosePlayerTurboWindow(true); });
    }

    private void RaiseCloseTurboDeckWindow() {
        PhotonNetwork.networkingPeer.OpRaiseEvent(EventCode.RequestCloseTurboWindow, "", true, RaiseEventOptions.Default);
    }

    private void RaiseGetTurboDeck() {
        PhotonNetwork.networkingPeer.OpRaiseEvent(EventCode.RequestTurboCards, "", true, RaiseEventOptions.Default);
    }

    public void ClosePlayerTurboWindow(bool pSendToServer) {
        if (_turboPlayerContainer.activeSelf) {
            foreach (Transform transform in _turboPlayerContainer.transform) {
                Destroy(transform.gameObject);
            }
            if (pSendToServer) {
                RaiseCloseTurboDeckWindow();
            }

            DisplayCards = false;
            _btnHide.gameObject.SetActive(false);
        }
    }

    public void CloseEnemyTurboWindow() {
        if (_turboEnemyContainer.activeSelf) {
            foreach (Transform transform in _turboEnemyContainer.transform) {
                Destroy(transform.gameObject);
            }
        }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (CardGameCore.Instance.GetActivePlayer().UserId == PhotonNetwork.player.UserId && !DisplayCards) { // Only allowed to play cards if it's your turn.
                RaiseGetTurboDeck();
                DisplayCards = true;
            } else {
                Debug.Log("Not your turn!");
            }
        }
    }

    public void DisplayTurboCards(Card[] pCards) {
        if (DisplayCards) {
            for (int i = 0; i < pCards.Length; i++) {
                GameObject turboCard = Instantiate(_turboPlayerCardPrefab, _turboPlayerContainer.transform);
                turboCard.transform.localPosition = new Vector3(-3.74f, 4.76f, 2.82f - (i * 2));
                turboCard.GetComponent<TurboCardGameBehaviour>().SetCardText(pCards[i].Data);
            }

            _btnHide.gameObject.SetActive(true);
        }
    }

    public void DisplayEnemyTurboCards(int pAmount) {
        for (int i = 0; i < pAmount; i++) {
            GameObject turboCard = Instantiate(_turboEnemyCardPrefab, _turboEnemyContainer.transform);
            turboCard.transform.localPosition = new Vector3(-1.22f, 4.18f, -6.18f + (i * 2));
        }
    }

    public void RemovePlayerTurboCard(int pIndex) {
        if (_turboPlayerContainer.transform.childCount >= pIndex) {
            Destroy(_turboPlayerContainer.transform.GetChild(pIndex).gameObject);
        }
    }

    public void RemoveEnemyTurboCard(int pIndex) {
        int childCount = _turboEnemyContainer.transform.childCount;
        if (childCount >= pIndex) {
            Destroy(_turboEnemyContainer.transform.GetChild(pIndex).gameObject);
        }
    }
}
