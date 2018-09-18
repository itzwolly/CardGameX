using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameHUDHandler : MonoBehaviour {
    [SerializeField] private Text _txtPlayerData;
    [SerializeField] private Text _turnTimer;
    [SerializeField] private GameObject _endTurnPopUp;
    [SerializeField] private GameObject _winPopUp;
    [SerializeField] private GameObject _losePopUp;

    private Coroutine _endTurnError;

    private void Start() {
        _txtPlayerData.text = "Player Data => Amount of players: " + PhotonNetwork.room.PlayerCount.ToString() + " | Room name: " + PhotonNetwork.room.Name + " | Player ID: " + PhotonNetwork.player.ID;
    }

    private void Update() {
        int timeLeft = (int) (Config.TURN_DURATION - TurnManager.Instance.ElapsedTimeInTurn);

        if (timeLeft > 0) {
            int currentTurn = TurnManager.Instance.CurrentTurn;
            string activeUserId = TurnManager.Instance.GetActivePlayer();

            _turnTimer.text = "Turn: " + currentTurn + " Time Left: " + timeLeft + " | Active player id: " + activeUserId;
        }
    }

    public void DisplayEndTurnError(float pTimeToWait) {
        if (_endTurnError != null) {
            StopCoroutine(_endTurnError);
            _endTurnError = null;
            _endTurnPopUp.gameObject.SetActive(false);
        }

        _endTurnError = StartCoroutine(EnableFadeOut(_endTurnPopUp, pTimeToWait));
    }

    public void EngageWinSequence(int pTimeToWait) {
        if (!_winPopUp.activeSelf) {
            _winPopUp.gameObject.SetActive(true);
        }

        StartCoroutine(EndGameSequence(pTimeToWait, true));
    }

    public void EngageLoseSequence(int pTimeToWait) {
        if (!_losePopUp.activeSelf) {
            _losePopUp.gameObject.SetActive(true);
        }

        StartCoroutine(EndGameSequence(pTimeToWait, false));
    }

    private IEnumerator EnableFadeOut(GameObject pObject, float pTimeToWait) {
        pObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(pTimeToWait);
        pObject.gameObject.SetActive(false);
    }

    private IEnumerator EndGameSequence(int pAmount, bool pHasWon) {
        yield return new WaitForSeconds(pAmount);
        // Leave current room.
        if (PhotonNetwork.connected) {
            PhotonNetwork.LeaveRoom();
        }
        // Not using boolean "pHasWon" for now, but will be useful in the future.
        // For displaying different particles when you lose vs when you win etc.
        LevelManager.Instance.LoadLevelASync(Config.MAIN_SCENE);
    }
}
