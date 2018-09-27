using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class NetworkManager : UnityEngine.MonoBehaviour {
    private Coroutine _crService;

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }
        
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.autoJoinLobby = false;

        if (PhotonNetwork.connectionState == ConnectionState.Disconnected) {
            // TODO: This is temporary, switching to custom authentication using the custom launcher.
            PhotonNetwork.AuthValues = new AuthenticationValues(/*SystemInfo.deviceUniqueIdentifier*/ Guid.NewGuid().ToString()); // swap with Guid.NewGuid().ToString() if you want to be able to play on the same device.
            Debug.Log("UserId: " + PhotonNetwork.AuthValues.UserId);

            PhotonNetwork.networkingPeer.Connect("127.0.0.1:4530", ServerConnection.MasterServer);
        }
    }

    private void Service() {
        //Debug.Log("Calling Service: " + (int) Time.time);
        // https://doc.photonengine.com/en-us/realtime/current/reference/client-connection-handling
        if (PhotonNetwork.networkingPeer != null) {
            PhotonNetwork.networkingPeer.Service();
        }
    }

    private void OnConnectedToMaster() {
        Debug.Log("Connected to master.");
        InvokeRepeating("Service", 0.001f, 0.05f); // 20x per second
    }

    private void OnJoinedRandomRoom() {
        Debug.Log("Joined random room. Currently: " + PhotonNetwork.room.PlayerCount + " player(s) waiting.");
    }

    private void OnJoinedRoom() {
        //Debug.Log("Joined room. Currently: " + PhotonNetwork.room.PlayerCount + " player(s) waiting. UserId: " + PhotonNetwork.player.UserId);

        //if (SceneManagerHelper.ActiveSceneName != Config.GAME_SCENE) {
        //    if (PhotonNetwork.room.PlayerCount == Config.MAX_PLAYERS) {
        //        Debug.Log("Player limit reached. Starting game...");

        //        if (PhotonNetwork.automaticallySyncScene) {
        //            for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++) {
        //                Events.RaiseJoinGameEvent(PhotonNetwork.playerList[i].ID);
        //                Debug.Log("Raised join game event..");
        //            }
        //        }
        //    }
        //}
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer pOther) {
        Debug.Log("Player " + pOther.ID + " Disconnected.");
        Events.RaiseEndGameEvent();
    }

    private void OnConnectionFail(DisconnectCause pCause) {
        Debug.Log("Cause for connection failure is: " + pCause);
        if (!PhotonNetwork.connected || PhotonNetwork.room == null) {
            PhotonNetwork.networkingPeer.ReconnectAndRejoin();
        }
    }

    private void OnDisconnectedFromPhoton() {
        Debug.Log("Disconnected from photon.");
        PhotonNetwork.networkingPeer.ReconnectAndRejoin();
    }

    private void OnLeftRoom() {
        Debug.Log("Left the room");
    }
    
    private void OnPhotonJoinRoomFailed() {
        Debug.Log("Failed to join a room.");

        if (SceneManagerHelper.ActiveSceneName != Config.MAIN_SCENE) {
            Debug.Log("Changing back to the main scene.");
            LevelManager.Instance.PhotonLoadLevelASync(Config.MAIN_SCENE);
        }
    }

    private void OnPhotonRandomJoinFailed() {
        Debug.Log("Failed to join a random room. Creating a new one...");
        EnterRoomParams roomParams = new EnterRoomParams();
        roomParams.RoomOptions = new RoomOptions() { MaxPlayers = Config.MAX_PLAYERS, PlayerTtl = Config.PLAYER_TTL, EmptyRoomTtl = Config.EMPTY_ROOM_TTL, CleanupCacheOnLeave = Config.CLEANUP_CACHE_ON_LEAVE, Plugins = Config.PLUGINS_NAME };

        PhotonNetwork.networkingPeer.OpCreateGame(roomParams);
    }
}
