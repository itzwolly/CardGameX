using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour {
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _spawnPoint;

    private void Start() {
        CreatePlayer();
    }

    public GameObject CreatePlayer() {
        return Instantiate(_playerPrefab, _spawnPoint.transform);
    }
}
