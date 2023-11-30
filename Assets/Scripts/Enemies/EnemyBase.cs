using UnityEngine;

public class EnemyBase : MonoBehaviour {

    public float health;

    [HideInInspector] public float hp;

    GameObject player;
    Vector3 pos;

    void Awake() {
        gameObject.SetActive(false);
    }

    void Start() {
        player = GameController.instance.GetPlayer();
        pos = gameObject.transform.position;
        hp = health;
    }

    void OnTriggerStay(Collider other) {
        if(other.gameObject == player) {
            player.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

    public void TakeDamage(float dmg) {
        hp -= dmg;
        if(hp <= 0) {
            Destroy(gameObject);
        }
    }

    public void Spawn() {
        hp = health;
        gameObject.SetActive(true);
        if(pos != Vector3.zero) {
            transform.position = pos;
        }
    }

    public void Despawn() {
        gameObject.SetActive(false);
    }

    void OnDestroy() {
        if(!gameObject.scene.isLoaded) {
            return;
        }
        RoomController.instance.RemoveEnemyFromList(this);
    }
}