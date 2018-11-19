using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {
    private static BoardManager _instance;

    public static BoardManager Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<BoardManager>();

                if (_instance == null) {
                    Debug.Log("No PlayerHandler in scene..");
                }
            }
            return _instance;
        }
    }

    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private GameObject[] _playerBoardSlots;
    [SerializeField] private GameObject[] _enemyBoardSlots;
    
    public GameObject[] GetBoardSlots(int pPlayerId) {
        if (pPlayerId == PhotonNetwork.player.ID) {
            return _playerBoardSlots;
        } else {
            return _enemyBoardSlots;
        }
    }

    public bool PlaceMonster(GameObject pSlot, MonsterCardData pMonsterCardData) {
        if (pSlot.transform.childCount == 0) {
            Debug.Log("Placing monster with: " + pMonsterCardData.Id + " | " + pMonsterCardData.Name);

            GameObject card = Instantiate(_cardPrefab, pSlot.transform);
            card.transform.localPosition = new Vector3(0, 0, -0.01f);
            card.transform.localRotation = Quaternion.identity;
            card.transform.localScale = new Vector3(1, 1, 1);

            card.GetComponent<BoardCardBehaviour>().SetMonsterCardText(pMonsterCardData);
            return true;
        }
        Debug.Log("Trying to place monster, but slot count is not equals to 0");
        return false;
    }
}
