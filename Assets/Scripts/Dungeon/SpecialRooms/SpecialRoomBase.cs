using System.Collections;
using UnityEngine;

public class SpecialRoomBase : MonoBehaviour {

    [SerializeField] Material doorMaterial;
    [SerializeField] int unlockCost;

    void Start() {
        StartCoroutine(WaitForGenerationEnd());
    }

    IEnumerator WaitForGenerationEnd() {
        while(RoomController.instance.currentRoom == null) {
            yield return null;
        }
        SetSpecialDoors();
    }

    void SetSpecialDoors() {
        gameObject.GetComponent<Room>().SetSpecialDoors(doorMaterial, unlockCost);
    }
}