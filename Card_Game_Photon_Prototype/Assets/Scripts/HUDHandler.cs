using UnityEngine;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour {
    [SerializeField] private Text _txtPlayerData;
    [SerializeField] private Text _turnTimer;

    private void Start() {
        _txtPlayerData.text = "Player Data => Amount of players: " + PhotonNetwork.room.PlayerCount.ToString() + " | Room name: " + PhotonNetwork.room.Name + " | Player ID: " + PhotonNetwork.player.ID;
    }

    private void Update() {
        int timeLeft = (int) (Config.TURN_DURATION - TurnManager.Instance.ElapsedTimeInTurn);

        if (timeLeft > 0) {
            _turnTimer.text = "Turn: " + TurnManager.Instance.CurrentTurn + " Time Left: " + timeLeft + " | Active player id: " + TurnManager.Instance.GetActivePlayer().ID;
        }
    }
}
