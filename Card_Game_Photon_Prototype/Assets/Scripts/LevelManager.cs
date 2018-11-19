using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void LoadLevelASync(int pLevel) {
        if (SceneManagerHelper.ActiveSceneBuildIndex != pLevel) {
            StartCoroutine(CRLoadLevelASync(pLevel));
        }
    }

    public void LoadLevelASync(string pLevel) {
        if (SceneManagerHelper.ActiveSceneName != pLevel) {
            StartCoroutine(CRLoadLevelASync(pLevel));
        }
    }

    public void LoadGameplaySceneAsync(Action pCallBack) {
        if (SceneManagerHelper.ActiveSceneBuildIndex != 1) {
            StartCoroutine(CRLoadGameplaySceneAsync(pCallBack));
        }
    }

    private IEnumerator CRLoadLevelASync(int pLevel) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(pLevel);

        while (!operation.isDone) {
            Debug.Log("Loading.. scene: " + pLevel + " progress: " + operation.progress);
            yield return null;
        }
    }

    private IEnumerator CRLoadLevelASync(string pLevel) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(pLevel);

        while (!operation.isDone) {
            Debug.Log("Loading.. scene: " + pLevel + " progress: " + operation.progress);
            yield return null;
        }
    }

    private IEnumerator CRLoadGameplaySceneAsync(Action pCallBack) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);

        while (!operation.isDone) {
            Debug.Log("Loading.. scene: " + 1 + " progress: " + operation.progress);
            yield return null;
        }

        pCallBack();
    }
}
