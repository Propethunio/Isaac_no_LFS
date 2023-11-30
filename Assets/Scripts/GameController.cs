using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;

    [SerializeField] float onRoomEnterFightDelay = .2f;

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
}