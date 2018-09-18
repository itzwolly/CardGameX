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

    public const string ACTIVE_PLAYER_KEY = "APK";

    public ITurnManagerCallbacks TurnManagerListener;
    public float TurnDuration = 5f;

    public bool IsActivePlayer {
        get { return (PhotonNetwork.player.UserId == GetActivePlayer()); }
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

    public void StartGame() {
        SetActivePlayer(PhotonNetwork.player.UserId);
        BeginTurn();
    }

    public void BeginTurn() {
        if (CurrentTurn > 0) {
            if (GetActivePlayer() != PhotonNetwork.player.UserId) {
                SetActivePlayer(PhotonNetwork.player.UserId);
            }
        }
        
        CurrentTurn = CurrentTurn + 1;
        TurnManagerListener.OnTurnBegins(CurrentTurn);
    }

    public bool EndTurn() {
        if (IsActivePlayer) {
            Events.RaiseEndTurnEvent(CurrentTurn);
            return true;
        }
        return false;
    }

    // Sets which player's turn it is right now.
    public void SetActivePlayer(string pUserId) {
        Room room = PhotonNetwork.room;
        if (room == null || room.CustomProperties == null) {
            return;
        }

        string propKey = ACTIVE_PLAYER_KEY;
        Hashtable playerData = new Hashtable();
        playerData[propKey] = pUserId;

        room.SetCustomProperties(playerData);
    }

    // Gets which player's turn it is right now.
    public string GetActivePlayer() {
        Room room = PhotonNetwork.room;
        if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(ACTIVE_PLAYER_KEY)) {
            return null;
        }

        return (string) room.CustomProperties[ACTIVE_PLAYER_KEY];
    }
}
