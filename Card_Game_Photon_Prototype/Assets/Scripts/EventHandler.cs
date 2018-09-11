using UnityEngine;
using ExitGames.Client.Photon;

public class EventHandler : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public void OnEvent(byte pEventCode, object pContent, int pSenderId) {
        object[] data = (object[]) pContent;
        PhotonPlayer sender = PhotonPlayer.Find(pSenderId);

        switch (pEventCode) {
            case Events.PLAY_CARD: {
                    int cardId = (int) data[0];
                    int cardIndex = (int) data[1];
                    TurnManager.Instance.TurnManagerListener.OnPlayerMove(sender, cardId, cardIndex);
                    break;
                }
            case Events.END_TURN: {
                    int turnIndex = (int) data[0];
                    TurnManager.Instance.TurnManagerListener.OnTurnEnds(turnIndex);
                    break;
                }
            case Events.JOIN_GAME: {
                    int playerId = (int) data[0];
                    if (playerId == PhotonNetwork.player.ID) {
                        StartCoroutine(LevelManager.Instance.LoadSceneAsync(Config.GAME_SCENE));
                    }
                    break;
                }
            default:
                break;
        }
    }

    public void OnEnable() {
        PhotonNetwork.OnEventCall += OnEvent;
    }

    public void OnDisable() {
        PhotonNetwork.OnEventCall -= OnEvent;
    }
}
