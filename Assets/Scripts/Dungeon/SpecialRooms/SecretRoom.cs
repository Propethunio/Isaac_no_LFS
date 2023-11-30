using System.Collections;
using UnityEngine;

public class SecretRoom : MonoBehaviour {

    void Start() {
        StartCoroutine(WaitForGenerationEnd());
    }

    IEnumerator WaitForGenerationEnd() {
        while(RoomController.instance.currentRoom == null) {
            yield return null;
        }
        SetDestructableWalls();
    }

    void SetDestructableWalls() {
        gameObject.GetComponent<Room>().SetSecretWalls();
    }
}