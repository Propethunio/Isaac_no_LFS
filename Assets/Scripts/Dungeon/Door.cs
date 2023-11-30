using UnityEngine;

public class Door : MonoBehaviour {

    public enum DoorType {
        top,
        down,
        left,
        right
    }

    [SerializeField] GameObject door;
    [SerializeField] MeshRenderer doorMesh;

    [HideInInspector] public bool isDestroyed, isLocked;
    [HideInInspector] public int unlockCost;

    public DoorType doorType;

    Room room;

    void Start() {
        room = GetComponentInParent<Room>();
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            Vector3 pos = new Vector3();
            switch(doorType) {
                case DoorType.top:
                    pos = room.topSpawn.position;
                    break;
                case DoorType.down:
                    pos = room.bottomSpawn.position;
                    break;
                case DoorType.left:
                    pos = room.leftSpawn.position;
                    break;
                case DoorType.right:
                    pos = room.rightSpawn.position;
                    break;
            }
            StartCoroutine(GameController.instance.PlayerEneterNewRoom(room, pos));
        }
    }

    public void Open() {
        if(!isLocked) {
            door.SetActive(false);
        }
    }

    public void Close() {
        if(!isDestroyed) {
            door.SetActive(true);
        }
    }

    public void TryDestroyDoor() {
        if(!isLocked) {
            isDestroyed = true;
            DestroyNextRoomDoor();
            Open();
        }
    }

    void GetNextRoomDoor(out int x, out int z, out DoorType nextRoomDoorType) {
        x = room.x;
        z = room.z;
        nextRoomDoorType = default;
        switch(doorType) {
            case DoorType.top:
                z++;
                nextRoomDoorType = DoorType.down;
                break;
            case DoorType.down:
                z--;
                nextRoomDoorType = DoorType.top;
                break;
            case DoorType.left:
                x--;
                nextRoomDoorType = DoorType.right;
                break;
            case DoorType.right:
                x++;
                nextRoomDoorType = DoorType.left;
                break;
        }
    }

    void DestroyNextRoomDoor() {
        int x, z;
        DoorType nextRoomDoorType;
        GetNextRoomDoor(out x, out z, out nextRoomDoorType);
        Room nextRoom = RoomController.instance.FindRoom(x, z);
        nextRoom.SetDoorDestroyed(nextRoomDoorType);
    }

    public void ChangeDoorMaterial(Material mat) {
        doorMesh.material = mat;
    }

    public void TryUnlockDoor() {
        if(isLocked && !room.DoesHaveEnemies()) {
            PlayerStats stats = GameController.instance.GetPlayer().GetComponent<PlayerStats>();
            if(stats.keysAmount >= unlockCost) {
                stats.keysAmount -= unlockCost;
                ItemsUIController.instance.SetKeys(stats.keysAmount);
                if(unlockCost > 0) {
                    SoundManager.instance.PlaySound(SoundManager.instance.useKey);
                }
                SoundManager.instance.PlaySound(SoundManager.instance.doorOpen);
                UnlockDoor();
                UnlockNextRoomDoor();
            }
        }
    }

    void UnlockNextRoomDoor() {
        int x, z;
        DoorType nextRoomDoorType;
        GetNextRoomDoor(out x, out z, out nextRoomDoorType);
        Room nextRoom = RoomController.instance.FindRoom(x, z);
        nextRoom.SetDoorUnlocked(nextRoomDoorType);
    }

    public void UnlockDoor() {
        isLocked = false;
        Open();
    }
}