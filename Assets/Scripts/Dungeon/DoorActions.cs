using UnityEngine;

public class DoorActions : MonoBehaviour {

    [SerializeField] Door door;

    public void DestroyDoor() {
        door.TryDestroyDoor();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            door.TryUnlockDoor();
        }
    }
}