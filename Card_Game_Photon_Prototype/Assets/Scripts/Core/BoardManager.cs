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

    [HideInInspector] public BoardCardBehaviour Attacker;
    [HideInInspector] public GameObject Target;

    public GameObject[] GetBoardSlots(int pPlayerId) {
        if (pPlayerId == PhotonNetwork.player.ID) {
            return _playerBoardSlots;
        } else {
            return _enemyBoardSlots;
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

    public void UpdateMonster(int pOwnerId, int pIndex, int pHealth) {
        Debug.Log(PhotonNetwork.player.UserId + " | " + pIndex + " | " + pOwnerId);

        GameObject[] slots = GetBoardSlots(pOwnerId);
        GameObject obj = slots[pIndex].transform.GetChild(0).gameObject;
        BoardCardBehaviour bcb = obj.GetComponent<BoardCardBehaviour>();

        MonsterCardData data = bcb.Data;
        data.Health = pHealth;
        bcb.Data = data;
    }
}
