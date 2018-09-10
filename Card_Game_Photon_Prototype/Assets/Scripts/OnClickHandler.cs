using UnityEngine;
using UnityEngine.UI;

public class OnClickHandler : MonoBehaviour {
    [SerializeField] private InputField _creatRoom;
    [SerializeField] private InputField _joinRoom;

    public void onClickCreateRoom() {
        PhotonNetwork.CreateRoom(_creatRoom.text, new RoomOptions() { MaxPlayers = Config.MAX_PLAYERS, PlayerTtl = Config.Player_TTL }, null);
    }

    public void onClickJoinRoom() {
        PhotonNetwork.JoinRoom(_joinRoom.text);
    }
}
