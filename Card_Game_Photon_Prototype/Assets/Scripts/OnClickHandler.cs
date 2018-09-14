using UnityEngine;
using UnityEngine.UI;

public class OnClickHandler : MonoBehaviour {
    [SerializeField] private InputField _creatRoom;
    [SerializeField] private InputField _joinRoom;

    public void onClickQuickPlay() {
        // Currently completely random, will add a filter later.
        // returns bool, to check if it couldn't find a room (i.e
        // If A is looking for a room, but decides to stop looking and B tries to join using quick play Error: OperationResponse 226: Returncode: 32758 is thrown)
        if (!PhotonNetwork.JoinRandomRoom()) {
            Debug.Log("No random room found!");
        } else {
            Debug.Log("Joined Existing room: " + PhotonNetwork.room);
        }
    }

    public void onClickCreateRoom() {
        PhotonNetwork.CreateRoom(_creatRoom.text, new RoomOptions() { MaxPlayers = Config.MAX_PLAYERS, PlayerTtl = Config.PLAYER_TTL, EmptyRoomTtl = Config.EMPTY_ROOM_TTL, CleanupCacheOnLeave = Config.CLEANUP_CACHE_ON_LEAVE }, null);
    }

    public void onClickJoinRoom() {
        PhotonNetwork.JoinRoom(_joinRoom.text);
    }
}
