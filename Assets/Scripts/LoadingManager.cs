using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour {

    [SerializeField] GameObject loadingPanel;
    [SerializeField] Slider loadingSlider;

    void Start() {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame() {
        int roomsToGenerate = DungeonDataGenerator.instance.roomsToGenerate;
        while (RoomController.instance.currentRoom == null) {
            float loadingProgress = RoomController.instance.loadedRooms.Count / (float)roomsToGenerate;
            loadingSlider.value = loadingProgress;
            yield return null;
        }
        loadingPanel.SetActive(false);
        GameManager.instance.StartGame();
    }
}