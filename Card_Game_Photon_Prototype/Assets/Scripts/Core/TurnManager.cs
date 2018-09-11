using Photon;
using UnityEngine;
using ExitGames.Client.Photon;

public class TurnManager : PunBehaviour {
    #region Singleton Initializing
    private static TurnManager _instance;

    public static TurnManager Instance
    {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<TurnManager>();

                if (_instance == null) {
                    GameObject container = new GameObject("Turn Manager");
                    _instance = container.AddComponent<TurnManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    private const string ACTIVE_PLAYER_KEY = "APK";
    private PhotonPlayer _activePlayer;

    public ITurnManagerCallbacks TurnManagerListener;
    public float TurnDuration = 5f;

    public bool IsActivePlayer {
        get { return (PhotonNetwork.player == GetActivePlayer()); }
    }

    public int CurrentTurn {
        get { return PhotonNetwork.room.GetTurn(); }
        private set {
            PhotonNetwork.room.SetTurn(value, true);
        }
    }

    public float ElapsedTimeInTurn {
        get { return ((float) (PhotonNetwork.ServerTimestamp - PhotonNetwork.room.GetTurnStart())) / 1000.0f; }
    }

    public float RemainingSecondsInTurn {
        get { return Mathf.Max(0f, TurnDuration - ElapsedTimeInTurn); }
    }

    public bool TurnTimeIsOver {
        get { return RemainingSecondsInTurn <= 0f; }
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (CurrentTurn > 0 && TurnTimeIsOver) {
            TurnManagerListener.OnTurnTimeEnds(CurrentTurn);
        }
    }

    public void BeginTurn() {
        // Sets the first client that joined as the active player on turn 1,
        // but for every following turn; checks if the local player is not the active player
        // and saves it as the active player.
        if (CurrentTurn == 0) {
            SetActivePlayer(PhotonNetwork.player);
        } else {
            if (GetActivePlayer() != PhotonNetwork.player) {
                SetActivePlayer(PhotonNetwork.player);
            }
        }
        
        CurrentTurn = CurrentTurn + 1;
    }

    public void EndTurn() {
        if (IsActivePlayer) {
            Events.RaiseEndTurnEvent(CurrentTurn);
        } else {
            Debug.Log("Sorry! It is not your turn..");
        }
    }

    // Sets which player's turn it is right now.
    public void SetActivePlayer(PhotonPlayer pPlayer) {
        Room room = PhotonNetwork.room;
        if (room == null || room.CustomProperties == null) {
            return;
        }

        string propKey = ACTIVE_PLAYER_KEY;
        Hashtable playerData = new Hashtable();
        playerData[propKey] = pPlayer;

        room.SetCustomProperties(playerData);
    }

    // Gets which player's turn it is right now.
    public PhotonPlayer GetActivePlayer() {
        Room room = PhotonNetwork.room;
        if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(ACTIVE_PLAYER_KEY)) {
            return null;
        }

        return (PhotonPlayer) room.CustomProperties[ACTIVE_PLAYER_KEY];
    }
}
