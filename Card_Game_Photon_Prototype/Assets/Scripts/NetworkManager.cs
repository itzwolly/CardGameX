using System;
using System.Collections;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        // This is temporary, switching to custom authentication using the custom launcher.
        PhotonNetwork.AuthValues = new AuthenticationValues(Guid.NewGuid().ToString());
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.autoJoinLobby = false;

        PhotonNetwork.ConnectUsingSettings(Config.VERSION);
        Debug.Log("Connected to version: " + Config.VERSION);
    }

    private void OnConnectedToMaster() {
        //PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected to master.");
    }

    //private void OnJoinedLobby() {
    //    Debug.Log("Joined lobby");
    //}

    private void OnJoinedRoom() {
        Debug.Log("Joined room. Currently: " + PhotonNetwork.room.PlayerCount + " player(s) waiting.");

        if (PhotonNetwork.room.PlayerCount == Config.MAX_PLAYERS) {
            Debug.Log("Player limit reached. Starting game...");

            if (PhotonNetwork.automaticallySyncScene) {
                for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++) {
                    Events.RaiseJoinGameEvent(PhotonNetwork.playerList[i].ID);
                }
            }
        }
    }

    private void OnConnectionFail(DisconnectCause pCause) {
        Debug.Log("Cause for connection failure is: " + pCause);
    }

    private void OnDisconnectedFromPhoton() {
        Debug.Log("Disconnected from photon.");

        if (!PhotonNetwork.ReconnectAndRejoin()) {
            Debug.LogError("Failed to reconnect to photon and rejoin to room");
        }
    }

    private void OnLeftRoom() {
        Debug.Log("Left the room");
    }
    
    private void OnPhotonJoinRoomFailed() {
        Debug.Log("Failed to join a room");
        Debug.Log("Currently in room: " + PhotonNetwork.room + " with build index: " + SceneManagerHelper.ActiveSceneBuildIndex);

        if (SceneManagerHelper.ActiveSceneBuildIndex != 0) {
            StartCoroutine(LevelManager.Instance.PhotonLoadLevelAsync(0));
        }
    }

    private void OnPhotonRandomJoinFailed() {
        Debug.Log("Failed to join a random room. Creating a new one...");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = Config.MAX_PLAYERS, PlayerTtl = Config.Player_TTL, EmptyRoomTtl = 3000 }, null);
    }
}
