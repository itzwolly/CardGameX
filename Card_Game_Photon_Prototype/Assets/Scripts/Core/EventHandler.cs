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

        Debug.Log("[Received] Event from the server: ev=" + pEventCode + ", data=" + (debug == "" ? "null" : debug) + "" + ", senderId=" + sender.ID + " on player.ID: " + PhotonNetwork.player.ID);

        switch (pEventCode) {
            case Events.PLAY_CARD: {
                    int cardId = (int) data[0];
                    int cardIndex = (int) data[1];
                    TurnManager.Instance.TurnManagerListener.OnPlayerMove(sender, cardId, cardIndex);
                    break;
                }
            case Events.END_TURN: {
                    TurnManager.Instance.TurnManagerListener.OnTurnEnd(sender);
                    break;
                }
            case Events.JOIN_GAME: {
                    int playerId = (int) data[0];
                    if (playerId == PhotonNetwork.player.ID) {
                        LevelManager.Instance.PhotonLoadLevelASync(Config.GAME_SCENE);
                    }
                    break;
                }
            case Events.END_GAME: {
                    TurnManager.Instance.TurnManagerListener.OnGameEnd(sender);
                    break;
                }
            case Events.CARD_DRAWN: {
                    int handSize = (int) data[0];
                    TurnManager.Instance.TurnManagerListener.OnCardDrawn(sender, handSize);
                    break;
                }
            case Events.START_TURN: {
                    TurnManager.Instance.TurnManagerListener.OnTurnBegins(sender);
                    break;
                }
            default:
                break;
        }
    }

    private void OnEnable() {
        Debug.Log("Started Listening to OnEventCall");
        PhotonNetwork.OnEventCall += OnEvent;
    }

    private void OnDisable() {
        Debug.Log("Stopped listening to OnEventCall");
        PhotonNetwork.OnEventCall -= OnEvent;
    }
}
