using System;
using System.Collections;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        // This is temporary, switching to custom authentication using the custom launcher.
        PhotonNetwork.AuthValues = new AuthenticationValues(Guid.NewGuid().ToString());
        PhotonNetwork.automaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings(Config.VERSION);
        Debug.Log("Connected to version: " + Config.VERSION);
    }

    private void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected to master.");
    }

    private void OnJoinedLobby() {
        Debug.Log("Joined lobby");
    }

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

    private void OnDisconnectedFromPhoton() {
        Debug.Log("Disconnected from photon.");
    }

    private void OnLeftRoom() {
        Debug.Log("Left the room");
    }
    
    private void OnPhotonJoinRoomFailed() {
        Debug.Log("Failed to join a room");
    }
}
