using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    [SerializeField] bool drawSizeGizmo;
    [SerializeField] GameObject topWall, bottomWall, leftWall, rightWall;

    [HideInInspector] public List<EnemyBase> enemiesList;
    [HideInInspector] public int x, z;

    public RewardController.RoomRewardType roomRewardType;
    public float width, height;
    public Transform topSpawn, bottomSpawn, leftSpawn, rightSpawn;

    List<Door> doorsList;

    public virtual void Start() {
        RoomController.instance.RegisterRoom(this);
        doorsList = new List<Door>(GetComponentsInChildren<Door>());
        enemiesList = new List<EnemyBase>(GetComponentsInChildren<EnemyBase>(true));
    }

    public virtual void StartFight() {
        CloseDoors();
        SpawnEnemies();
    }

    void SpawnEnemies() {
        enemiesList.ForEach(enemy => { enemy.Spawn(); });
    }

    public void DespawnEnemies() {
        enemiesList.ForEach(enemy => { enemy.Despawn(); });
    }

    public bool DoesHaveEnemies() {
        return enemiesList.Count > 0;
    }

    public void RemoveEnemyFromList(EnemyBase enemy) {
        enemiesList.Remove(enemy);
        if(!DoesHaveEnemies()) {
            OpenDoors();
            SpawnReward();
        }
    }

    public virtual void SpawnReward() {
        GameObject reward = RewardController.instance.CheckForRoomReward(roomRewardType);
        if(reward != null) {
            Instantiate(reward, new Vector3(transform.position.x, .25f, transform.position.z), reward.transform.rotation);
        }
    }

    public void RemoveUnconnectedDoors() {
        Vector2Int dir = new Vector2Int();
        foreach(Door door in doorsList) {
            switch(door.doorType) {
                case Door.DoorType.top:
                    dir = Vector2Int.up;
                    break;
                case Door.DoorType.down:
                    dir = Vector2Int.down;
                    break;
                case Door.DoorType.left:
                    dir = Vector2Int.left;
                    break;
                case Door.DoorType.right:
                    dir = Vector2Int.right;
                    break;
            }
            if(!IsRoomConnected(dir)) {
                door.gameObject.SetActive(false);
                ActivateWall(dir);
            }
        }
    }

    void ActivateWall(Vector2Int dir) {
        if(dir == Vector2Int.up) {
            topWall.SetActive(true);
        } else if(dir == Vector2Int.down) {
            bottomWall.SetActive(true);
        } else if(dir == Vector2Int.left) {
            leftWall.SetActive(true);
        } else {
            rightWall.SetActive(true);
        }
    }

    bool IsRoomConnected(Vector2Int dir) {
        return RoomController.instance.DoesRoomExist(x + dir.x, z + dir.y);
    }

    public Vector3 GetRoomCenter() {
        return new Vector3(x * width, 0, z * height);
    }

    public void OpenDoors() {
        SoundManager.instance.PlaySound(SoundManager.instance.doorOpen);
        foreach(Door door in doorsList) {
            door.Open();
        }
    }

    void CloseDoors() {
        SoundManager.instance.PlaySound(SoundManager.instance.doorClose);
        foreach(Door door in doorsList) {
            door.Close();
        }
    }

    public void SetDoorDestroyed(Door.DoorType doorType) {
        foreach(Door door in doorsList) {
            if(door.doorType == doorType) {
                door.isDestroyed = true;
            }
        }
    }

    public void SetDoorUnlocked(Door.DoorType doorType) {
        foreach(Door door in doorsList) {
            if(door.doorType == doorType) {
                door.UnlockDoor();
            }
        }
    }

    void GetNextRoomDoor(Door.DoorType doorType, out int x, out int z, out Door.DoorType nextRoomDoorType) {
        x = this.x;
        z = this.z;
        nextRoomDoorType = default;
        switch(doorType) {
            case Door.DoorType.top:
                z++;
                nextRoomDoorType = Door.DoorType.down;
                break;
            case Door.DoorType.down:
                z--;
                nextRoomDoorType = Door.DoorType.top;
                break;
            case Door.DoorType.left:
                x--;
                nextRoomDoorType = Door.DoorType.right;
                break;
            case Door.DoorType.right:
                x++;
                nextRoomDoorType = Door.DoorType.left;
                break;
        }
    }

    public void SetSpecialDoors(Material mat, int unlockCost) {
        foreach(Door door in doorsList) {
            int x, z;
            Door.DoorType nextRoomDoor;
            GetNextRoomDoor(door.doorType, out x, out z, out nextRoomDoor);
            Room nextRoom = RoomController.instance.FindRoom(x, z);
            if(nextRoom != null && nextRoom.GetComponent<SecretRoom>() == null) {
                SetSpecialDoor(door, mat, unlockCost);
                nextRoom.SetNextRoomDoor(nextRoomDoor, mat, unlockCost);
            }
        }
    }

    void SetNextRoomDoor(Door.DoorType doorType, Material mat, int unlockCost) {
        foreach(Door door in doorsList) {
            if(door.doorType == doorType) {
                SetSpecialDoor(door, mat, unlockCost);
                break;
            }
        }
    }

    void SetSpecialDoor(Door door, Material mat, int unlockCost) {
        door.ChangeDoorMaterial(mat);
        door.unlockCost = unlockCost;
        door.isLocked = true;
        door.Close();
    }

    void ActivateSecretWall(Door.DoorType doorType) {
        GameObject wall = null;
        switch(doorType) {
            case Door.DoorType.top:
                wall = topWall;
                break;
            case Door.DoorType.down:
                wall = bottomWall;
                break;
            case Door.DoorType.left:
                wall = leftWall;
                break;
            case Door.DoorType.right:
                wall = rightWall;
                break;
        }
        wall.SetActive(true);
        wall.tag = "SecretWall";
        SecretRoomWall secretWall = wall.AddComponent<SecretRoomWall>();
        secretWall.doorType = doorType;
        secretWall.x = this.x;
        secretWall.z = this.z;
    }

    public void SetSecretWalls() {
        foreach(Door door in doorsList) {
            int x, z;
            Door.DoorType nextRoomDoor;
            GetNextRoomDoor(door.doorType, out x, out z, out nextRoomDoor);
            Room nextRoom = RoomController.instance.FindRoom(x, z);
            if(nextRoom != null) {
                DeactivateDoorVisuals(door);
                ActivateSecretWall(door.doorType);
                nextRoom.SetNextRoomSecretWall(nextRoomDoor);
            }
        }
    }

    void DeactivateDoorVisuals(Door door) {
        foreach(Transform child in door.transform) {
            child.gameObject.SetActive(false);
        }
    }

    void SetNextRoomSecretWall(Door.DoorType doorType) {
        foreach(Door door in doorsList) {
            if(door.doorType == doorType) {
                DeactivateDoorVisuals(door);
                ActivateSecretWall(door.doorType);
            }
        }
    }

    public void DestroySecretWall(Door.DoorType doorType) {
        switch(doorType) {
            case Door.DoorType.top:
                topWall.SetActive(false);
                break;
            case Door.DoorType.down:
                bottomWall.SetActive(false); ;
                break;
            case Door.DoorType.left:
                leftWall.SetActive(false); ;
                break;
            case Door.DoorType.right:
                rightWall.SetActive(false); ;
                break;
        }
    }

    void OnDrawGizmos() {
        if(drawSizeGizmo) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(width, 0.1f, height));
        }
    }
}