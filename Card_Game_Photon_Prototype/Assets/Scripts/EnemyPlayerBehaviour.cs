using ExitGames.Client.Photon;
using UnityEngine;

public class EnemyPlayerBehaviour : MonoBehaviour {

    private void SendAttackEvent(Hashtable pData) {
        PhotonNetwork.networkingPeer.OpRaiseEvent(EventCode.Attack, pData, true, RaiseEventOptions.Default);
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (CardGameCore.Instance.GetActivePlayer().UserId == PhotonNetwork.player.UserId) {
                if (BoardManager.Instance.Attacker != null) {
                    BoardManager.Instance.Target = gameObject;

                    int targetId = CardGameCore.Instance.Opponent.ActorNr;

                    int attackerId = BoardManager.Instance.Attacker.Data.Id;
                    int attackerOwnerId = CardGameCore.Instance.GetActivePlayer().ActorNr;
                    int attackerIndex = GetSiblingIndex(BoardManager.Instance.Attacker.transform.parent, BoardManager.Instance.Attacker.transform.parent.parent);

                    Hashtable data = new Hashtable();
                    data.Add("targetid", targetId);

                    data.Add("attackerid", attackerId);
                    data.Add("attackerownerid", attackerOwnerId);
                    data.Add("attackerindex", attackerIndex);

                    SendAttackEvent(data);

                    BoardManager.Instance.Attacker = null;
                    BoardManager.Instance.Target = null;
                }
            } else {
                Debug.Log("Not your turn..");
            }
        }
    }

    private int GetSiblingIndex(Transform child, Transform parent) {
        for (int i = 0; i < parent.childCount; ++i) {
            if (child == parent.GetChild(i)) {
                return i;
            }
        }
        Debug.LogWarning("Child doesn't belong to this parent.");
        return 0;
    }
}
