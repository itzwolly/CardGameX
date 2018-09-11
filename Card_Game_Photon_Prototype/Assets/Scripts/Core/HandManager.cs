using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

    [SerializeField] private GameObject _playerHand;
    [SerializeField] private GameObject _enemyHand;

	// Use this for initialization
	private void Start () {
        PhotonNetwork.player.TagObject = this;
	}

    public void DestroyCard(int pIndex) {
        if (TurnManager.Instance.IsActivePlayer) {
            Destroy(_playerHand.transform.GetChild(pIndex).gameObject);
        } else {
            Destroy(_enemyHand.transform.GetChild((_enemyHand.transform.childCount - 1) - pIndex).gameObject);
        }
    }
}
