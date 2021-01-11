using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurboCardGameBehaviour : MonoBehaviour {
    private CardData _data;
    private int _index;

    public int Index {
        get { return _index; }
    }

    [SerializeField] private Text _txtName;
    [SerializeField] private InputField _txtDescription;
    [SerializeField] private Text _txtCost;
    [SerializeField] private Text _txtAttack;
    [SerializeField] private Text _txtHealth;

    public string CardName {
        get { return _txtName.text; }
    }

    public void SetCardText(CardData pCardData) {
        _data = pCardData;

        _txtName.text = pCardData.Name;
        _txtDescription.text = pCardData.Description;
        _txtCost.text = pCardData.TurboCost.ToString();
        _txtAttack.text = "";
        _txtHealth.text = "";

        if (pCardData is MonsterCardData) {
            MonsterCardData mcd = pCardData as MonsterCardData;

            _txtAttack.text = mcd.Attack.ToString();
            _txtHealth.text = mcd.Health.ToString();
        }
    }

    private void SendAddTurboCardEvent(int pCardId, int pIndex) {
        Hashtable data = new Hashtable();
        data.Add("cardid", pCardId);
        data.Add("cardindex", pIndex);

        PhotonNetwork.networkingPeer.OpRaiseEvent(EventCode.AddTurboCard, data, true, RaiseEventOptions.Default);
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (transform.tag == "Interactable") {
                if (CardGameCore.Instance.GetActivePlayer().UserId == PhotonNetwork.player.UserId) { // Only allowed to play cards if it's your turn.
                    _index = GetSiblingIndex(transform, transform.parent);

                    // TODO: ADD ON CLICK BEHAVIOUR
                    SendAddTurboCardEvent(_data.Id, _index);
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
