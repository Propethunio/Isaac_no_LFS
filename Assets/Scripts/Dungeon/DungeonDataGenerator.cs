using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonDataGenerator : MonoBehaviour {

    public static DungeonDataGenerator instance;

    [SerializeField] SceneField startScene;
    [SerializeField] List<SceneField> bossScenes;
    [SerializeField] List<SceneField> treasureScenes;
    [SerializeField] List<SceneField> storeScenes;
    [SerializeField] List<SceneField> secretScenes;
    [SerializeField] List<SceneField> easyRoomScenes;
    [SerializeField] List<SceneField> normalRoomScenes;
    [SerializeField] List<SceneField> hardRoomScenes;
    [SerializeField] List<SceneField> specialRoomScenes;

    [SerializeField] int normalRooms, hardRooms, specialRooms;

    public int roomsToGenerate = 12;

    List<Vector2Int> dungeonRoomsList = new List<Vector2Int>(), potentialRoomsList = new List<Vector2Int>(), specialRoomsList = new List<Vector2Int>();
    Vector2Int bossRoom, storeRoom, treasureRoom, secretRoom;
    int failedGenerations;

    void Awake() {
        instance = this;
    }

    void Start() {
        GenerateDungeonData();
        GenerateSpecialRooms();
        SpawnRooms();
    }

    void GenerateSpecialRooms() {
        bool roomsSet = false;
        while(!roomsSet) {
            List<Vector2Int> potentialEndRoomsList = new List<Vector2Int>();
            List<Vector2Int> potentialSecretRoomsList = new List<Vector2Int>();
            List<Vector2Int> roomsListCopy = new List<Vector2Int>(dungeonRoomsList);

            // Remove start and start neighbors 
            roomsListCopy.RemoveAll(dir => dir == Vector2Int.zero || dir == Vector2Int.up || dir == Vector2Int.down || dir == Vector2Int.left || dir == Vector2Int.right);

            // Generate end rooms (special rooms)
            foreach(Vector2Int room in roomsListCopy) {
                if(NeighborCount(room) == 1) {
                    potentialEndRoomsList.Add(room);
                }
            }
            if(potentialEndRoomsList.Count < 3 + specialRooms) {
                RestartGeneration();
                continue;
            } else {
                bossRoom = potentialEndRoomsList[Random.Range(0, potentialEndRoomsList.Count)];
                potentialEndRoomsList.Remove(bossRoom);
                roomsListCopy.Remove(bossRoom);
                storeRoom = potentialEndRoomsList[Random.Range(0, potentialEndRoomsList.Count)];
                potentialEndRoomsList.Remove(storeRoom);
                treasureRoom = potentialEndRoomsList[Random.Range(0, potentialEndRoomsList.Count)];
                potentialEndRoomsList.Remove(treasureRoom);
                while(specialRooms > 0) {
                    specialRooms--;
                    Vector2Int temporaryRoom = potentialEndRoomsList[Random.Range(0, potentialEndRoomsList.Count)];
                    specialRoomsList.Add(temporaryRoom);
                    potentialEndRoomsList.Remove(temporaryRoom);
                }
            }

            // Generate secret room
            List<Vector2Int> emptyNeighbors = new List<Vector2Int>();
            foreach(Vector2Int room in roomsListCopy) {
                emptyNeighbors.AddRange(GetEmptyNeighbors(room)?.Except(emptyNeighbors) ?? new List<Vector2Int>());
            }
            foreach(Vector2Int room in emptyNeighbors) {
                if(NeighborCount(room) > 2 && !AdjacentToBossRoom(room)) {
                    potentialSecretRoomsList.Add(room);
                }
            }
            if(potentialSecretRoomsList.Count < 1) {
                RestartGeneration();
                continue;
            } else {
                secretRoom = potentialSecretRoomsList[Random.Range(0, potentialSecretRoomsList.Count)];
                roomsSet = true;
            }
        }
    }

    bool AdjacentToBossRoom(Vector2Int room) {
        return bossRoom == room + Vector2Int.up
            || bossRoom == room + Vector2Int.down
            || bossRoom == room + Vector2Int.left
            || bossRoom == room + Vector2Int.right;
    }

    List<Vector2Int> GetEmptyNeighbors(Vector2Int room) {
        return CreateDirectionList(room).Where(dir => !dungeonRoomsList.Contains(dir)).ToList();
    }

    void SpawnRooms() {
        RemoveSpecialRoomsFromBasics();
        RoomController.instance.AddRoomToQueue(startScene, 0, 0);
        RoomController.instance.AddRoomToQueue(PickRandomSceneFromList(bossScenes), bossRoom.x, bossRoom.y);
        RoomController.instance.AddRoomToQueue(PickRandomSceneFromList(storeScenes), storeRoom.x, storeRoom.y);
        RoomController.instance.AddRoomToQueue(PickRandomSceneFromList(treasureScenes), treasureRoom.x, treasureRoom.y);
        RoomController.instance.AddRoomToQueue(PickRandomSceneFromList(secretScenes), secretRoom.x, secretRoom.y);
        specialRoomsList.ForEach(room => RoomController.instance.AddRoomToQueue(PickRandomSceneFromList(specialRoomScenes), room.x, room.y));
        dungeonRoomsList.ForEach(room => RoomController.instance.AddRoomToQueue(PickBaseRoom(), room.x, room.y));
        RoomController.instance.StartLoadingRoomsFromQueue();
    }

    SceneField PickBaseRoom() {
        if(dungeonRoomsList.Count > hardRooms + normalRooms && Random.value > .5f) {
            return PickRandomSceneFromList(easyRoomScenes);
        }
        if(hardRooms > 0) {
            hardRooms--;
            return PickRandomSceneFromList(hardRoomScenes);
        }
        if(normalRooms > 0) {
            normalRooms--;
            return PickRandomSceneFromList(normalRoomScenes);
        }
        return PickRandomSceneFromList(easyRoomScenes);
    }

    SceneField PickRandomSceneFromList(List<SceneField> list) {
        int index = Random.Range(0, list.Count);
        SceneField scene = list[index];
        list.Remove(scene);
        return scene;
    }

    void RemoveSpecialRoomsFromBasics() {
        specialRoomsList.ForEach(room => dungeonRoomsList.Remove(room));
        dungeonRoomsList.Remove(treasureRoom);
        dungeonRoomsList.Remove(bossRoom);
        dungeonRoomsList.Remove(storeRoom);
        dungeonRoomsList.Remove(Vector2Int.zero);
    }

    void GenerateDungeonData() {
        if(failedGenerations >= 100) {
            failedGenerations = 0;
            roomsToGenerate++;
        }
        List<Vector2Int> directions = CreateDirectionList(Vector2Int.zero);
        dungeonRoomsList.Add(Vector2Int.zero);
        potentialRoomsList.AddRange(directions);
        GenerateRoomsList();
    }

    void GenerateRoomsList() {
        while(potentialRoomsList.Count > 0) {
            foreach(Vector2Int room in potentialRoomsList.ToList()) {
                TryAddRoom(room);
                potentialRoomsList.Remove(room);
            }
        }
        if(dungeonRoomsList.Count < roomsToGenerate - 1) {
            TryStartNewPath();
        }
    }

    void TryStartNewPath() {
        List<Vector2Int> possibleNewPaths = CreateDirectionList(Vector2Int.zero).Where(dir => !dungeonRoomsList.Contains(dir) && NeighborCount(dir) == 1).ToList();
        if(possibleNewPaths.Count > 0) {
            possibleNewPaths.ForEach(path => potentialRoomsList.Add(path));
            GenerateRoomsList();
        } else {
            RestartGeneration();
        }
    }

    void RestartGeneration() {
        failedGenerations++;
        dungeonRoomsList.Clear();
        GenerateDungeonData();
    }

    void TryAddRoom(Vector2Int room) {
        if(dungeonRoomsList.Count >= roomsToGenerate - 1
            || Random.value > .5f
            || dungeonRoomsList.Contains(room)
            || NeighborCount(room) > 1
        ) return;
        dungeonRoomsList.Add(room);
        potentialRoomsList.AddRange(CreateDirectionList(room));
    }

    int NeighborCount(Vector2Int room) {
        return CreateDirectionList(room).Count(dir => dungeonRoomsList.Contains(dir));
    }

    List<Vector2Int> CreateDirectionList(Vector2Int startDir) {
        return new List<Vector2Int> {
            Vector2Int.up + startDir,
            Vector2Int.down + startDir,
            Vector2Int.left + startDir,
            Vector2Int.right + startDir
        };
    }
}