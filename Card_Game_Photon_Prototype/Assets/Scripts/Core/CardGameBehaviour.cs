using UnityEngine;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class CardGameBehaviour : MonoBehaviour {
    private int _cardId;

    public int CardId {
        get { return _cardId; }
        set { _cardId = value; }
    }

    private Text _data;

    private void Start() {
        _data = GetComponentInChildren<Text>();
    }

    private void Update() {
        if (_data != null) {
            if (_data.text != _cardId.ToString()) {
                _data.text = _cardId.ToString();
            }
        }
    }

    private void SendPlayCardEvent(int pCardId, int pIndex) {
        RaiseEventOptions options = new RaiseEventOptions();
        options.Receivers = ReceiverGroup.All;
        options.TargetActors = null;
        options.InterestGroup = 0;
        options.CachingOption = EventCaching.AddToRoomCache;

        Hashtable data = new Hashtable();
        data.Add("cardid", pCardId);
        data.Add("cardindex", pIndex);

        PhotonNetwork.networkingPeer.OpRaiseEvent(EventCode.UpdateGameState, data, true, options);
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (transform.tag == "Interactable") {
                if (TurnManager.Instance.GetActivePlayer() == PhotonNetwork.player.UserId) { // Only allowed to play cards if it's your turn.
                    SendPlayCardEvent(_cardId, transform.GetSiblingIndex());
                } else {
                    TurnManager.Instance.TurnManagerListener.OnNotYourTurn(PhotonNetwork.player);
                }
            }
        }
    }
}
