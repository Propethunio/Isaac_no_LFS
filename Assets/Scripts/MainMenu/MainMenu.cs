using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] SceneField gameScene;

    void Start() {
        SoundManager.instance.ChangeMusic(SoundManager.instance.mainMenuMusic);
    }

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