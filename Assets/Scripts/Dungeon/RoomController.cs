using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomInfo {
    public string name;
    public int x, z;
}

public class RoomController : MonoBehaviour {

    public static RoomController instance;

    [SerializeField] NavMeshSurface surface;

    [HideInInspector] public Room currentRoom;

    List<Room> loadedRooms = new List<Room>();
    string currentWorldName = "Basement";
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    RoomInfo currentLoadRoomData;

    void Awake() {
        instance = this;
    }

    public void StartLoadingRoomsFromQueue() {
        StartCoroutine(LoadRoomsFromQueue());
    }

    IEnumerator LoadRoomsFromQueue() {
        while(loadRoomQueue.Count > 0) {
            currentLoadRoomData = loadRoomQueue.Dequeue();
            yield return StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
        }
        foreach(Room room in loadedRooms) {
            room.RemoveUnconnectedDoors();
        }
        currentRoom = FindRoom(0, 0);
        surface.BuildNavMesh();
    }

    IEnumerator LoadRoomRoutine(RoomInfo info) {
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(info.name, LoadSceneMode.Additive);
        while(!loadRoom.isDone) {
            yield return null;
        }
    }

    public void AddRoomToQueue(string name, int x, int z) {
        RoomInfo newRoomData = new RoomInfo() {
            name = name,
            x = x,
            z = z
        };
        loadRoomQueue.Enqueue(newRoomData);
    }

    public void RegisterRoom(Room room) {
        room.transform.position = new Vector3(currentLoadRoomData.x * room.width, 0, currentLoadRoomData.z * room.height);
        room.x = currentLoadRoomData.x;
        room.z = currentLoadRoomData.z;
        room.name = $"{currentWorldName}-{currentLoadRoomData.name} {room.x}, {room.z}";
        room.transform.parent = transform;
        loadedRooms.Add(room);
    }

    public bool DoesRoomExist(int x, int y) {
        return FindRoom(x, y) != null;
    }

    public Room FindRoom(int x, int y) {
        return loadedRooms.Find(item => item.x == x && item.z == y);
    }

    public IEnumerator PlayerEnterDoor(Room room, float time) {
        if(currentRoom.DoesHaveEnemies()) {
            currentRoom.DespawnEnemies();
        }
        currentRoom = room;
        if(room.DoesHaveEnemies()) {
            yield return new WaitForSeconds(time);
            room.StartFight();
        }
    }

    public void RemoveEnemyFromList(EnemyBase enemy) {
        currentRoom.RemoveEnemyFromList(enemy);
    }
}