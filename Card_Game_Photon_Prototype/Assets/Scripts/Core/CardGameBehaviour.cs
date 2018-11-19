using UnityEngine;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class CardGameBehaviour : MonoBehaviour {
    private int _cardId;
    private int _index;

    public int CardId {
        get { return _cardId; }
        set { _cardId = value; }
    }
    public int Index {
        get { return _index; }
        //set { _index = value; }
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
        _txtName.text = pCardData.Name;
        _txtDescription.text = pCardData.Description;
        _txtCost.text = pCardData.RegCost.ToString();
        _txtAttack.text = "";
        _txtHealth.text = "";

        if (pCardData is MonsterCardData) {
            MonsterCardData mcd = pCardData as MonsterCardData;

            _txtAttack.text = mcd.Attack.ToString();
            _txtHealth.text = mcd.Health.ToString();
        }
    }

    private void SendPlaySpellCardEvent(int pCardId, int pIndex) {
        RaiseEventOptions options = new RaiseEventOptions();
        options.Receivers = ReceiverGroup.All;
        options.TargetActors = null;
        options.InterestGroup = 0;
        options.CachingOption = EventCaching.AddToRoomCache;

        Hashtable data = new Hashtable();
        data.Add("cardid", pCardId);
        data.Add("cardindex", pIndex);

        PhotonNetwork.networkingPeer.OpRaiseEvent(EventCode.PlaySpellCard, data, true, options);
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp(0)) {
            if (transform.tag == "Interactable") {
                if (CardGameCore.Instance.GetActivePlayer().UserId == PhotonNetwork.player.UserId) { // Only allowed to play cards if it's your turn.
                    _index = GetSiblingIndex(transform, transform.parent);
                    Debug.Log("Cards count: " + Hand.Instance.Cards.Count + " | Index: " + _index);

                    Card selectedCard = Hand.Instance.Cards[_index];
                    if (selectedCard.IsMonster()) {
                        Hand.Instance.SelectedCardBehaviour = this;
                    } else {
                        SendPlaySpellCardEvent(selectedCard.Data.Id, _index);
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
