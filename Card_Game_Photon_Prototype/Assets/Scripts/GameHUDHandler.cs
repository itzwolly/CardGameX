using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameHUDHandler : MonoBehaviour {
    [SerializeField] private Text _txtPlayerData;
    [SerializeField] private Text _turnTimer;
    [SerializeField] private Text _activePlayerText;
    [SerializeField] private Text _playerHealth;
    [SerializeField] private Text _enemyHealth;
    [SerializeField] private GameObject _endTurnPopUp;
    [SerializeField] private GameObject _winPopUp;
    [SerializeField] private GameObject _losePopUp;
    [SerializeField] private Button _btnEndTurn;

    private Coroutine _endTurnError;

    public Button btnEndTurn {
        get { return _btnEndTurn; }
    }
    public Text ActivePlayerText {
        get { return _activePlayerText; }
    }
    public Text PlayerHealth {
        get { return _playerHealth; }
    }
    public Text EnemyHealth {
        get { return _enemyHealth; }
    }

    private static GameHUDHandler _instance;
    public static GameHUDHandler Instance {
        get {
            if (!_instance) {
                _instance = FindObjectOfType(typeof(GameHUDHandler)) as GameHUDHandler;

                if (!_instance) {
                    Debug.Log("There should be an active object with the component Hand");
                } else {
                    _instance.Init();
                }
            }
            return _instance;
        }
    }

    public void Init() {
        _btnEndTurn.onClick.AddListener(TurnManager.Instance.EndTurn);

        _txtPlayerData.text = "Data => Amount of players: " + PhotonNetwork.room.PlayerCount.ToString() + " | Room name: " + PhotonNetwork.room.Name + " | Player UserId: " + PhotonNetwork.player.UserId;
    }

    private void Update() {
        int timeLeft = (int) (Config.TURN_DURATION - TurnManager.Instance.ElapsedTimeInTurn);

        if (timeLeft > 0) {
            //int currentTurn = TurnManager.Instance.CurrentTurn;
            /*string activeUserId = TurnManager.Instance.GetActivePlayer();*/

            _turnTimer.text = " Time Left: " + timeLeft;
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
