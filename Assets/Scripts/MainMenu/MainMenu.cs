using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] SceneField gameScene;

    public void Play() {
        StartCoroutine(LoadGame());
    }

    public void Quit() {
        Application.Quit();
    }

    IEnumerator LoadGame() {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(gameScene);
        while(loadingOperation.isDone) {
            yield return null;
        }
    }
}