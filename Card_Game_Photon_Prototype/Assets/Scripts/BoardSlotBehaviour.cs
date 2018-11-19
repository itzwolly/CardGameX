using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class BoardSlotBehaviour : MonoBehaviour {
    
    private int _boardIndex = 0;
    private bool _occupied = false;

    private void Start() {
        _boardIndex = transform.GetSiblingIndex();   
    }

    private void SendPlayMonsterCardEvent(int pCardId, int pIndex, int pBoardIndex) {
        RaiseEventOptions options = new RaiseEventOptions();
        options.Receivers = ReceiverGroup.All;
        options.TargetActors = null;
        options.InterestGroup = 0;
        options.CachingOption = EventCaching.AddToRoomCache;

        Hashtable data = new Hashtable();
        data.Add("cardid", pCardId); // what card
        data.Add("cardindex", pIndex); // the index of the card in your hand (so the other player knows which card will be removed)
        data.Add("boardindex", pBoardIndex); // the board slot that the player selected.

        PhotonNetwork.networkingPeer.OpRaiseEvent(EventCode.PlayMonsterCard, data, true, options);
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (CardGameCore.Instance.GetActivePlayer().UserId == PhotonNetwork.player.UserId) { // Only allowed to play cards if it's your turn.
                if (!_occupied) {
                    CardGameBehaviour selectedCard = Hand.Instance.SelectedCardBehaviour;
                    if (selectedCard != null) {
                        int cardIndex = selectedCard.Index;
                        if (cardIndex >= 0) {
                            Card card = Hand.Instance.Cards[cardIndex];
                            SendPlayMonsterCardEvent(card.Data.Id, cardIndex, _boardIndex);

                            Hand.Instance.SelectedCardBehaviour = null;
                            _occupied = true;
                        }
                    }
                } else {
                    // TODO: SLOT OCCUPIED
                }
            } else {
                Debug.Log("Not your turn!");
            }
        }
    }
}
