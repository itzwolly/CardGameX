using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour {
    [SerializeField] private GameObject[] _playerBoardSlots;
    [SerializeField] private GameObject[] _enemyBoardSlots;

    public void OnCardPlayed(byte eventCode, object content, int senderId) {
        if (eventCode == Events.PLAY_CARD) {
            object[] data = (object[]) content;

            int targetIndex = (int) data[0];
            int playerIndex = (int) data[1];

            if (playerIndex == PhotonNetwork.player.ID) {
                _playerBoardSlots[targetIndex].GetComponent<BoardSlotBehaviour>().OccupyBoardSlot();
            } else {
                _enemyBoardSlots[(_playerBoardSlots.Length - 1) - targetIndex].GetComponent<BoardSlotBehaviour>().OccupyBoardSlot();
            }
        }
    }

    public void OnEnable() {
        PhotonNetwork.OnEventCall += OnCardPlayed;
    }

    public void OnDisable() {
        PhotonNetwork.OnEventCall -= OnCardPlayed;
    }
}
