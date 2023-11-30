using UnityEngine;

public class TreasureRoom : MonoBehaviour {

    void Start() {
        GameObject item = RewardController.instance.RollFromPowerUps();
        Instantiate(item, new Vector3(transform.position.x, .25f, transform.position.z), Quaternion.identity, this.transform);
    }
}