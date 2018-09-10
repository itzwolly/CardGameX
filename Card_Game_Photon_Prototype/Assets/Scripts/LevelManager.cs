using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    private static LevelManager _instance;

    public static LevelManager Instance {
        get {
             if (_instance == null) {
                 _instance = GameObject.FindObjectOfType<LevelManager>();
             
                 if (_instance == null) {
                     GameObject container = new GameObject("Level Manager");
                     _instance = container.AddComponent<LevelManager>();
                 }
             }
             return _instance;
         }
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadSceneAsync(int pLevel) {
        AsyncOperation operation = PhotonNetwork.LoadLevelAsync(pLevel);

        while (!operation.isDone) {
            Debug.Log("Loading.. scene: " + pLevel + " progress: " + operation.progress);
            yield return null;
        }
    }

    public IEnumerator LoadSceneAsync(string pLevel) {
        AsyncOperation operation = PhotonNetwork.LoadLevelAsync(pLevel);

        while (!operation.isDone) {
            Debug.Log("Loading.. scene: " + pLevel + " progress: " + operation.progress);
            yield return null;
        }
    }
}
