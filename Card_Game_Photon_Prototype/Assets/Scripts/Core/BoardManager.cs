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
    [SerializeField] private GameObject _playerBoard;
    [SerializeField] private GameObject _enemyBoard;

    [HideInInspector] public BoardCardBehaviour Attacker;
    [HideInInspector] public GameObject Target;

    public Transform GetBoardById(int pPlayerId) {
        if (pPlayerId == PhotonNetwork.player.ID) {
            return _playerBoard.transform;
        } else {
            return _enemyBoard.transform;
        }
    }

    public bool PlaceMonster(GameObject pSlot, Card pCard) {
        if (pSlot.transform.childCount == 0) {
            MonsterCardData data = pCard.Data as MonsterCardData;
            Debug.Log("Placing monster with: " + data.Id + " | " + data.Name);

            GameObject card = Instantiate(_cardPrefab, pSlot.transform);
            card.transform.localPosition = new Vector3(0, 0, -0.01f);
            card.transform.localRotation = Quaternion.identity;
            card.transform.localScale = new Vector3(1, 1, 1);

            card.GetComponent<BoardCardBehaviour>().SetMonsterCardText(pCard);
            return true;
        }
        Debug.Log("Trying to place monster, but slot count is not equals to 0");
        return false;
    }

    public BoardCardBehaviour GetBoardCardBehaviour(int pOwnerId, int pIndex) {
        Transform parent = GetBoardById(pOwnerId);
        if (pIndex < 0 || pIndex >= parent.childCount) {
            Debug.Log("Index out of bounds: " + pIndex);
            return null;
        }
        
        GameObject slot = parent.GetChild(pIndex).gameObject;
        if (slot != null) {
            //Debug.Log("GameObject : " + slot.name + " has " + slot.transform.childCount + " Children");
            GameObject target = slot.transform.GetChild(0).gameObject;
            BoardCardBehaviour bcb = target.GetComponent<BoardCardBehaviour>();
            return bcb;
        }
        return null;
    }
}
