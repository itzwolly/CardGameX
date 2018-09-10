using ExitGames.Client.Photon;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _spawnPoint;

    [SerializeField] private GameObject[] _playerBoardSlots;
    [SerializeField] private GameObject[] _enemyBoardSlots;
    
    private void Start() {
        CreatePlayer();
        PhotonNetwork.player.TagObject = this;
    }

    public GameObject CreatePlayer() {
        GameObject player = Instantiate(_playerPrefab, _spawnPoint.transform);
        return player;
    }

    public GameObject[] GetBoardSlots(int pTarget) {
        return (pTarget == 0) ? _playerBoardSlots : _enemyBoardSlots;
    }
}
