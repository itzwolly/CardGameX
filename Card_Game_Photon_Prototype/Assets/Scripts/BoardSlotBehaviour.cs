using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class BoardSlotBehaviour : MonoBehaviour {
    
    private int _boardIndex = 0;
    [HideInInspector] public bool Occupied = false;

    private void Start() {
        _boardIndex = transform.GetSiblingIndex();   
    }

    private void SendPlayMonsterCardEvent(int pCardId, int pIndex, int pBoardIndex) {
        Hashtable data = new Hashtable();
        data.Add("cardid", pCardId); // what card
        data.Add("cardindex", pIndex); // the index of the card in your hand (so the other player knows which card will be removed)
        data.Add("boardindex", pBoardIndex); // the board slot that the player selected.

        Debug.Log(PhotonNetwork.player.ID + " | " + pBoardIndex);

        PhotonNetwork.networkingPeer.OpRaiseEvent(EventCode.PlayMonsterCard, data, true, RaiseEventOptions.Default);
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (CardGameCore.Instance.GetActivePlayer().UserId == PhotonNetwork.player.UserId) { // Only allowed to play cards if it's your turn.
                if (!Occupied) {
                    CardGameBehaviour selectedCard = Hand.Instance.SelectedCardBehaviour;
                    if (selectedCard != null) {
                        int cardIndex = selectedCard.Index;
                        if (cardIndex >= 0) {
                            Card card = Hand.Instance.Cards[cardIndex];
                            SendPlayMonsterCardEvent(card.Data.Id, cardIndex, _boardIndex);

                            Hand.Instance.SelectedCardBehaviour = null;
                            Occupied = true;
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
