using UnityEngine;

public class SecretRoomWall : MonoBehaviour {

    public Door.DoorType doorType;
    public int x, z;

    public void DestroyWall() {
        gameObject.SetActive(false);
        Door.DoorType nextRoomDoorType = default;
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
        Room nextRoom = RoomController.instance.FindRoom(x, z);
        nextRoom.DestroySecretWall(nextRoomDoorType);
    }
}