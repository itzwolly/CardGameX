using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

    [SerializeField] private GameObject _playerHand;
    [SerializeField] private GameObject _enemyHand;

    [SerializeField] private GameObject _playerCardPrefab;
    [SerializeField] private GameObject _enemyCardPrefab;

    [SerializeField] private float offset = 1.66f;

    private List<GameObject> _playerHandSlots;
    private List<GameObject> _enemyHandSlots;

    // Use this for initialization
    private void Awake () {
        _playerHandSlots = new List<GameObject>();
        _enemyHandSlots = new List<GameObject>();

        PhotonNetwork.player.TagObject = this;
	}

    public void DestroyCard(int pPlayerId, int pIndex) {
        if (pPlayerId == PhotonNetwork.player.ID) {
            if (pIndex != _playerHandSlots.Count - 1) {
                for (int i = 0; i < _playerHandSlots.Count; i++) {
                    if (i >= pIndex) {
                        GameObject obj = _playerHandSlots[i];
                        Vector3 gameCardSize = obj.GetComponent<Renderer>().bounds.size;
                        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, obj.transform.localPosition.z + (gameCardSize.z * offset));
                    }
                }
            }

            GameObject gameObj = _playerHand.transform.GetChild(pIndex).gameObject;
            _playerHandSlots.Remove(gameObj);
            Destroy(gameObj);

            if (Hand.Instance.Cards[pIndex].Data is SpellCardData) {
                Hand.Instance.Cards.RemoveAt(pIndex);
            }
        } else {
            if (pIndex != _enemyHandSlots.Count - 1) {
                for (int i = 0; i < _enemyHandSlots.Count; i++) {
                    if (i >= pIndex) {
                        GameObject obj = _enemyHandSlots[i];
                        Vector3 gameCardSize = obj.GetComponent<Renderer>().bounds.size;
                        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, obj.transform.localPosition.z - (gameCardSize.z * offset));
                    }
                }
            }

            GameObject gameObj = _enemyHand.transform.GetChild(pIndex).gameObject;
            _enemyHandSlots.Remove(gameObj);
            Destroy(gameObj);
        }
    }

    public GameObject DisplayPlayerCardDrawn(int pHandsize) {
        if (_playerHandSlots.Count >= Config.MAX_HAND_SIZE) {
            return null;
        }

        GameObject gameCard = Instantiate(_playerCardPrefab, _playerHand.transform);
        Vector3 gameCardSize = gameCard.transform.GetComponent<Renderer>().bounds.size;
        Vector3 globalPosition = new Vector3(_playerHand.transform.position.x, _playerHand.transform.position.y, _playerHand.transform.position.z + ((-(pHandsize - 1)) * (gameCardSize.z * offset)));
        gameCard.transform.position = globalPosition;

        _playerHandSlots.Add(gameCard);
        return gameCard;
    }

    public GameObject DisplayEnemyCardDrawn(int pHandSize) {
        if (_enemyHandSlots.Count >= Config.MAX_HAND_SIZE) {
            return null;
        }

        GameObject gameCard = Instantiate(_enemyCardPrefab, _enemyHand.transform);
        Vector3 gameCardSize = gameCard.transform.GetComponent<Renderer>().bounds.size;
        Vector3 globalPosition = new Vector3(_enemyHand.transform.position.x, _enemyHand.transform.position.y, _enemyHand.transform.position.z + ((pHandSize - 1) * (gameCardSize.z * offset)));
        gameCard.transform.position = globalPosition;

        _enemyHandSlots.Add(gameCard);
        return gameCard;
    }
}
