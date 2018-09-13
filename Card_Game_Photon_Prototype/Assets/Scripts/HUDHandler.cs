using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour {
    [SerializeField] private Text _txtPlayerData;
    [SerializeField] private Text _turnTimer;
    [SerializeField] private GameObject _endTurnPopUp;
    [SerializeField] private GameObject _winPopUp;

    private Coroutine _endTurnError;

    private void Start() {
        _txtPlayerData.text = "Player Data => Amount of players: " + PhotonNetwork.room.PlayerCount.ToString() + " | Room name: " + PhotonNetwork.room.Name + " | Player ID: " + PhotonNetwork.player.ID;
    }

    private void Update() {
        int timeLeft = (int) (Config.TURN_DURATION - TurnManager.Instance.ElapsedTimeInTurn);

        if (timeLeft > 0) {
            int currentTurn = TurnManager.Instance.CurrentTurn;
            PhotonPlayer activePlayer = TurnManager.Instance.GetActivePlayer();

            if (activePlayer != null) {
                _turnTimer.text = "Turn: " + currentTurn + " Time Left: " + timeLeft + " | Active player id: " + activePlayer.ID;
            }
        }
    }

    public void DisplayEndTurnError(float pTimeToWait) {
        if (_endTurnError != null) {
            StopCoroutine(_endTurnError);
            _endTurnPopUp.gameObject.SetActive(false);
        }

        _endTurnError = StartCoroutine(EnableFadeOut(_endTurnPopUp, pTimeToWait));
    }

    public void DisplayWinMessage() {
        if (!_winPopUp.activeSelf) {
            _winPopUp.gameObject.SetActive(true);
        }
    }

    private IEnumerator EnableFadeOut(GameObject pObject, float pTimeToWait) {
        pObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(pTimeToWait);
        pObject.gameObject.SetActive(false);
    }
}
