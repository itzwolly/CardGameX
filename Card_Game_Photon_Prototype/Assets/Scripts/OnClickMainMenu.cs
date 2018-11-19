using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class OnClickMainMenu : MonoBehaviour {
    [SerializeField] private InputField _createRoom;
    [SerializeField] private InputField _joinRoom;

    public void onClickQuickPlay() {
        // Currently completely random, will add a filter later.
        OpJoinRandomRoomParams roomParams = new OpJoinRandomRoomParams();
        roomParams.MatchingType = MatchmakingMode.RandomMatching;

        if (DeckHandler.Instance.SelectedDeck != null) {
            PhotonNetwork.networkingPeer.OpJoinRandomRoom(roomParams);
        } else {
            Debug.Log("No deck selected..");
        }
    }

    public void onClickCreateRoom() {
        PhotonNetwork.CreateRoom(_createRoom.text, new RoomOptions() { MaxPlayers = Config.MAX_PLAYERS, PlayerTtl = Config.PLAYER_TTL, EmptyRoomTtl = Config.EMPTY_ROOM_TTL, CleanupCacheOnLeave = Config.CLEANUP_CACHE_ON_LEAVE, Plugins = Config.PLUGINS_NAME }, null);
    }

    public void onClickJoinRoom() {
        PhotonNetwork.JoinRoom(_joinRoom.text);
    }

    public void onClickCardCollection() {
        LevelManager.Instance.LoadLevelASync(Config.CARD_COLLECTION_SCENE);
    }
}
