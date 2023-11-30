using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController instance;

    [SerializeField] float moveSpeed;

    void Awake() {
        instance = this;
    }

    public IEnumerator UpdatePos(Room room) {
        Vector3 targetPos = room.GetRoomCenter();
        targetPos.y = transform.position.y;
        while(!transform.position.Equals(targetPos)) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}