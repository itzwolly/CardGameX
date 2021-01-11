using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Text;

public class NetworkManager : UnityEngine.MonoBehaviour {
    private Coroutine _crService;

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }

        string cmdUsername = GetCommandLineArgs("-username");
        string username = (cmdUsername == null) ? "user_1" : cmdUsername;

        Debug.Log("CMDUsername: " + cmdUsername + " | " + " Username: " + username);

        if (username != null && username != "") {
            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.autoJoinLobby = false;

            if (PhotonNetwork.connectionState == ConnectionState.Disconnected) {
                PhotonNetwork.AuthValues = new AuthenticationValues(username);

                //PhotonNetwork.networkingPeer.Connect("25.11.213.108:4530", ServerConnection.MasterServer);
                PhotonNetwork.networkingPeer.Connect("127.0.0.1:4530", ServerConnection.MasterServer);
            }
        } else {
            Debug.Log("Username NOT set");
        }
        
        //byte[] byteCode = new byte[] {
        //    0x01,
        //    1,
        //    0x13
        //};
        
        //string byteCodeString = Convert.ToBase64String(byteCode);
        //Debug.Log(byteCodeString);

        //var base64EncodedBytes = Convert.FromBase64String(byteCodeString);
        //string s = "";
        //foreach (byte b in base64EncodedBytes) {
        //    s += b + ",";
        //}
        //Debug.Log(s);
    }

    private void Service() {
        //Debug.Log("Calling Service: " + (int) Time.time);
        // https://doc.photonengine.com/en-us/realtime/current/reference/client-connection-handling
        if (PhotonNetwork.networkingPeer != null) {
            PhotonNetwork.networkingPeer.Service();
        }
    }

    private void OnConnectedToMaster() {
        Debug.Log("Connected to master: " + PhotonNetwork.player.ID + " | " + PhotonNetwork.player.UserId);
        InvokeRepeating("Service", 0.001f, 0.05f); // 20x per second
    }

    private void OnJoinedRandomRoom() {
        Debug.Log("Joined random room. Currently: " + PhotonNetwork.room.PlayerCount + " player(s) waiting.");
    }

    private void OnJoinedRoom() {
        //
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer pOther) {
        Debug.Log("Player " + pOther.ID + " Disconnected.");
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
            LevelManager.Instance.LoadLevelASync(Config.MAIN_SCENE);
        }
    }

    private void OnPhotonRandomJoinFailed() {
        Debug.Log("Failed to join a random room. Creating a new one...");
        EnterRoomParams roomParams = new EnterRoomParams();
        roomParams.RoomOptions = new RoomOptions() { MaxPlayers = Config.MAX_PLAYERS, PlayerTtl = Config.PLAYER_TTL, EmptyRoomTtl = Config.EMPTY_ROOM_TTL, CleanupCacheOnLeave = Config.CLEANUP_CACHE_ON_LEAVE, Plugins = Config.PLUGINS_NAME };
        PhotonNetwork.networkingPeer.OpCreateGame(roomParams);
    }

    private static string GetCommandLineArgs(string pArgumentName) {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++) {
            if (args[i] == pArgumentName && args.Length > i + 1) {
                return args[i + 1];
            }
        }
        return null;
    }
}
