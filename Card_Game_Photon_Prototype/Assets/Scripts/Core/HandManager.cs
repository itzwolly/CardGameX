using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

    [SerializeField] private GameObject _playerHand;
    [SerializeField] private GameObject _enemyHand;

    [SerializeField] private GameObject _playerCardPrefab;
    [SerializeField] private GameObject _enemyCardPrefab;

    [SerializeField] private float offset = 1.25f;

    // Use this for initialization
    private void Awake () {
        PhotonNetwork.player.TagObject = this;
	}

    public void DestroyCard(int pPlayerId, int pIndex) {
        if (pPlayerId == PhotonNetwork.player.ID) {
            Destroy(_playerHand.transform.GetChild(pIndex).gameObject);

            if (pIndex < _playerHand.transform.childCount - 1) {
                foreach (Transform child in _playerHand.transform) {
                    child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y, child.transform.position.z + offset);
                }
            }
        } else {
            Destroy(_enemyHand.transform.GetChild(pIndex).gameObject);

            if (pIndex < _enemyHand.transform.childCount - 1) {
                foreach (Transform child in _enemyHand.transform) {
                    child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y, child.transform.position.z - offset);
                }
            }
        }
    }

    public GameObject DisplayPlayerCardDrawn(int pHandsize) {
        GameObject gameCard = Instantiate(_playerCardPrefab, _playerHand.transform);
        Vector3 gameCardSize = gameCard.transform.GetComponent<Renderer>().bounds.size;
        Vector3 globalPosition = new Vector3(_playerHand.transform.position.x, _playerHand.transform.position.y, _playerHand.transform.position.z + ((-(pHandsize - 1)) * (gameCardSize.z * offset)));
        gameCard.transform.position = globalPosition;
        gameCard.GetComponent<Renderer>().material.color = Color.green;
        return gameCard;
    }

    public GameObject DisplayEnemyCardDrawn(int pHandSize) {
        GameObject gameCard = Instantiate(_enemyCardPrefab, _enemyHand.transform);
        Vector3 gameCardSize = gameCard.transform.GetComponent<Renderer>().bounds.size;
        Vector3 globalPosition = new Vector3(_enemyHand.transform.position.x, _enemyHand.transform.position.y, _enemyHand.transform.position.z + ((pHandSize - 1) * (gameCardSize.z * offset)));
        gameCard.transform.position = globalPosition;
        gameCard.GetComponent<Renderer>().material.color = Color.cyan;
        return gameCard;
    }
}
