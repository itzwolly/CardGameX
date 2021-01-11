using UnityEngine;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardGameBehaviour : MonoBehaviour {
    [SerializeField] private Text _txtName;
    [SerializeField] private InputField _txtDescription;
    [SerializeField] private Text _txtCost;
    [SerializeField] private Text _txtAttack;
    [SerializeField] private Text _txtHealth;

    private int _cardId;
    private int _index;

    public int CardId {
        get { return _cardId; }
        set { _cardId = value; }
    }
    public int Index {
        get { return _index; }
    }

    public string CardName {
        get { return _txtName.text; }
    }

    public void SetCardText(Card pCard) {
        CardData data = pCard.Data;

        _txtName.text = data.Name;
        _txtDescription.text = data.Description;
        _txtCost.text = data.RegCost.ToString();
        _txtAttack.text = "";
        _txtHealth.text = "";

        if (data is MonsterCardData) {
            MonsterCardData mcd = data as MonsterCardData;

            _txtAttack.text = mcd.Attack.ToString();
            _txtHealth.text = mcd.Health.ToString();
        }
    }

    private void SendPlaySpellCardEvent(Hashtable pData) {
        PhotonNetwork.networkingPeer.OpRaiseEvent(EventCode.PlaySpellCard, pData, true, RaiseEventOptions.Default);
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (transform.tag == "Interactable") {
                if (CardGameCore.Instance.GetActivePlayer().UserId == PhotonNetwork.player.UserId) { // Only allowed to play cards if it's your turn.
                    _index = GetSiblingIndex(transform, transform.parent);
                    Debug.Log("Cards count: " + Hand.Instance.Cards.Count + " | Index: " + _index);

                    Card selectedCard = Hand.Instance.Cards[_index];
                    if (selectedCard.IsMonster()) {
                        Hand.Instance.SelectedMonsterCardBehaviour = this;
                    } else {
                        if (Hand.Instance.SelectedSpellCardBehaviour == null) {
                            Hand.Instance.SelectedSpellCardBehaviour = this;
                        } else {
                            List<IInteractable> targets = Hand.Instance.GetTargets();
                            Hashtable data = new Hashtable();
                            data.Add("cardid", selectedCard.Data.Id);
                            data.Add("cardindex", _index);
                            data.Add("targetamount", targets.Count);

                            for (int i = 0; i < targets.Count; i++) {
                                IInteractable target = targets[i];

                                int id = target.GetId();
                                int index = target.GetBoardIndex();
                                int ownerId = target.GetOwnerId();

                                data.Add("target_id-" + i, id);
                                data.Add("target_index-" + i, index);
                                data.Add("target_ownerid-" + i, ownerId);
                            }

                            Debug.Log("Sending data to server with count: " + data.Count);

                            SendPlaySpellCardEvent(data);

                            Hand.Instance.GetTargets().Clear();
                            Hand.Instance.SelectedSpellCardBehaviour = null;
                        }
                    }
                } else {
                    Debug.Log("Not your turn!");
                }
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
