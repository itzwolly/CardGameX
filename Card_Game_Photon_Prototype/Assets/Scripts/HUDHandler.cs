using UnityEngine;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour {
    [SerializeField] private Text _txtPlayercount;

    private void Start() {
        _txtPlayercount.text = "Player Data => Amount of players: " + PhotonNetwork.room.PlayerCount.ToString() + " | Room name: " + PhotonNetwork.room.Name + " | Player ID: " + PhotonNetwork.player.ID;
    }
}
