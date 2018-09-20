using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

    [SerializeField] private GameObject _playerHand;
    [SerializeField] private GameObject _enemyHand;

    [SerializeField] private GameObject _playerCardPrefab;
    [SerializeField] private GameObject _enemyCardPrefab;

	// Use this for initialization
	private void Awake () {
        PhotonNetwork.player.TagObject = this;
	}

    public void DestroyCard(int pPlayerId, int pIndex) {
        if (pPlayerId == PhotonNetwork.player.ID) {
            Destroy(_playerHand.transform.GetChild(pIndex).gameObject);
        } else {
            Destroy(_enemyHand.transform.GetChild((_enemyHand.transform.childCount - 1) - pIndex).gameObject);
        }
    }

    public GameObject DisplayPlayerCardDrawn(int pHandSize) {
        GameObject gameCard = Instantiate(_playerCardPrefab, _playerHand.transform);

        Vector3 gameCardSize = gameCard.transform.GetComponent<Renderer>().bounds.size;
        Vector3 globalPosition = new Vector3(_playerHand.transform.position.x, _playerHand.transform.position.y, _playerHand.transform.position.z + ((-(pHandSize - 1)) * (gameCardSize.z * 1.25f)));
        gameCard.transform.position = globalPosition;

        return gameCard;
    }

    public GameObject DisplayEnemyCardDrawn(int pHandSize) {
        GameObject gameCard = Instantiate(_enemyCardPrefab, _enemyHand.transform);

        Vector3 gameCardSize = gameCard.transform.GetComponent<Renderer>().bounds.size;
        Vector3 globalPosition = new Vector3(_enemyHand.transform.position.x, _enemyHand.transform.position.y, _enemyHand.transform.position.z + ((pHandSize - 1) * (gameCardSize.z * 1.25f)));
        gameCard.transform.position = globalPosition;

        return gameCard;
    }
}
