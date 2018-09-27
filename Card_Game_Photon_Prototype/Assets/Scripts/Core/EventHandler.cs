using UnityEngine;
using ExitGames.Client.Photon;
using System;

public class EventHandler : MonoBehaviour {

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }
    }

    public void OnEvent(byte pEventCode, object pContent, int pSenderId) {
        object[] data = (object[]) pContent;
        PhotonPlayer sender = PhotonPlayer.Find(pSenderId);

        string debug = "";
        if (data != null) {
            for (int i = 0; i < data.Length; i++) {
                debug += data[i] + " {type = " + data[i].GetType() + "}" + ((i == data.Length - 1) ? "" : ", ");
            }
        }

        //Debug.Log("[Received] Event from the server: ev=" + pEventCode + ", data=" + (debug == "" ? "null" : debug) + "" + ", senderId=" + sender.ID + " on player.ID: " + PhotonNetwork.player.ID);

        switch (pEventCode) {
            case Events.JOIN_GAME: {
                    string playerId = (string) data[0];
                    if (playerId == PhotonNetwork.player.UserId) {
                        //LevelManager.Instance.PhotonLoadLevelASync(Config.GAME_SCENE);
                        PhotonNetwork.LoadLevelAsync(Config.GAME_SCENE);
                    }
                    break;
                }
            case Events.END_GAME:
            case Events.PLAY_CARD:
            case Events.END_TURN:
            case Events.CARD_DRAWN:
            case Events.START_TURN: 
            case Events.ADD_CARD_TO_DECK:
            default:
                break;
        }
    }

    private void OnEnable() {
        Debug.Log("Started Listening to OnEventCall");
        //PhotonNetwork.OnEventCall += OnEvent;
    }

    private void OnDisable() {
        Debug.Log("Stopped listening to OnEventCall");
        //PhotonNetwork.OnEventCall -= OnEvent;
    }
}
