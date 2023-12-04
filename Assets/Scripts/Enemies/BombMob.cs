using System.Collections.Generic;
using UnityEngine;

public class BombMob : MonoBehaviour {

    [SerializeField] float exlosionRadius, exlosionDamageToEnemy;
    [SerializeField] ParticleSystem particle;

    GameObject player;

    void Start() {
        player = GameManager.instance.GetPlayer();
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject == player) {
            Destroy(gameObject);
        }
    }

    void OnDestroy() {
        if(!gameObject.scene.isLoaded) {
            return;
        }
        Instantiate(particle, transform.position, Quaternion.identity);
        SoundManager.instance.PlaySound(SoundManager.instance.explosion);
        InteractWithObjectInRadius();
    }

    void InteractWithObjectInRadius() {
        List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(transform.position, exlosionRadius));
        foreach(Collider collider in colliders) {
            DealDamage(collider);
        }
    }

    void DealDamage(Collider collider) {
        if(collider.tag == "Player") {
            collider.GetComponent<PlayerHealth>().TakeDamage(1);
        } else if(collider.tag == "Enemy") {
            collider.GetComponent<EnemyBase>().TakeDamage(exlosionDamageToEnemy);
        } else if(collider.tag == "Door") {
            collider.GetComponent<DoorActions>().DestroyDoor();
        } else if(collider.tag == "SecretWall") {
            collider.GetComponent<SecretRoomWall>().DestroyWall();
        } else if(collider.tag == "Destructable") {
            Destroy(collider.gameObject);
        }
    }
}