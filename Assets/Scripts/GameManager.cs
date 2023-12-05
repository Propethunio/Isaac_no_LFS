using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [SerializeField] float onRoomEnterFightDelay = .2f;
    [SerializeField] TMP_Text endGameText;
    [SerializeField] GameObject endGamePanel;
    [SerializeField] SceneField mainMenuScene;

    GameObject player;

    void Awake() {
        instance = this;
    }

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public GameObject GetPlayer() {
        return player;
    }

    public IEnumerator PlayerEneterNewRoom(Room room, Vector3 pos) {
        PlayerInput input = player.GetComponent<PlayerInput>();
        input.DisablePlayerInput();
        player.GetComponent<PlayerMovement>().MoveToPosition(pos);
        yield return StartCoroutine(CameraController.instance.UpdatePos(room));
        yield return StartCoroutine(RoomController.instance.PlayerEnterDoor(room, onRoomEnterFightDelay));
        input.EnablePlayerInput();
    }

    public void TeleportPlayer(Vector3 pos) {
        player.GetComponent<PlayerInput>().DisablePlayerInput();
        player.GetComponent<PlayerMovement>().TweenToPosition(pos);
    }

    public void StartGame() {
        player.GetComponent<PlayerInput>().EnablePlayerInput();
        SoundManager.instance.ChangeMusic(SoundManager.instance.gameMusic);
    }

    public void PlayerWin() {
        endGameText.text = "You Win!";
        EndGame();
    }

    public void PlayerDied() {
        endGameText.text = "You Lose!";
        EndGame();
    }

    void EndGame() {
        Time.timeScale = 0f;
        StatisticManager.instance.SetStats();
        endGamePanel.SetActive(true);
        SoundManager.instance.ChangeMusic(SoundManager.instance.endMusic);
    }

    public void BackToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}