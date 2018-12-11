using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardCardBehaviour : MonoBehaviour {
    private MonsterCardData _data;

    public Text txtName;
    public InputField txtDescription;
    public Text txtCost;
    public Text txtAttack;
    public Text txtHealth;

    public MonsterCardData Data {
        get { return _data; }
        set {
            _data = value;
            SetMonsterCardText(_data);

            if (_data.Health <= 0) {
                BoardSlotBehaviour bsb = GetComponentInParent<BoardSlotBehaviour>();
                if (bsb != null) {
                    bsb.Occupied = false;
                }
                Destroy(gameObject);
            }
        }
    }

    public void SetMonsterCardText(Card pCard) {
        _data = pCard.Data as MonsterCardData;

        txtName.text = _data.Name;
        txtDescription.text = _data.Description;
        txtCost.text = _data.RegCost.ToString();
        txtAttack.text = _data.Attack.ToString();
        txtHealth.text = _data.Health.ToString();
    }

    public void SetMonsterCardText(MonsterCardData pData) {
        if (pData != null) {
            txtName.text = pData.Name;
            txtDescription.text = pData.Description;
            txtCost.text = pData.RegCost.ToString();
            txtAttack.text = pData.Attack.ToString();
            txtHealth.text = pData.Health.ToString();
        } else {
            Debug.Log("Setting monster card text recieved null!");
        }
    }

    private void SendAttackEvent(Hashtable pData) {
        PhotonNetwork.networkingPeer.OpRaiseEvent(EventCode.Attack, pData, true, RaiseEventOptions.Default);
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (CardGameCore.Instance.GetActivePlayer().UserId == PhotonNetwork.player.UserId) {
                if (Hand.Instance.SelectedSpellCardBehaviour != null) {
                    List<IInteractable> datas = Hand.Instance.GetTargets();
                    if (!datas.Contains(_data)) {
                        Hand.Instance.GetTargets().Add(_data);

                        Debug.Log("Selecting target!!");
                    }
                } else {
                    if (BoardManager.Instance.Attacker != null) {
                        BoardManager.Instance.Target = gameObject;

                        int targetId = BoardManager.Instance.Target.GetComponent<BoardCardBehaviour>().Data.Id;
                        int targetOwnerId = CardGameCore.Instance.Opponent.ActorNr;
                        int targetIndex = GetSiblingIndex(BoardManager.Instance.Target.transform.parent, BoardManager.Instance.Target.transform.parent.parent);
                        targetIndex = Mathf.Abs(targetIndex - (BoardManager.Instance.Target.transform.parent.parent.childCount - 1));


                        int attackerId = BoardManager.Instance.Attacker.Data.Id;
                        int attackerOwnerId = CardGameCore.Instance.GetActivePlayer().ActorNr;
                        int attackerIndex = GetSiblingIndex(BoardManager.Instance.Attacker.transform.parent, BoardManager.Instance.Attacker.transform.parent.parent);

                        Hashtable data = new Hashtable();
                        data.Add("targetid", targetId);
                        data.Add("targetownerid", targetOwnerId);
                        data.Add("targetindex", targetIndex);

                        data.Add("attackerid", attackerId);
                        data.Add("attackerownerid", attackerOwnerId);
                        data.Add("attackerindex", attackerIndex);

                        SendAttackEvent(data);

                        BoardManager.Instance.Attacker = null;
                        BoardManager.Instance.Target = null;
                    }

                    BoardManager.Instance.Attacker = this;
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
