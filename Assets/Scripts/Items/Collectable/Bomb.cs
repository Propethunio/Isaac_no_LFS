using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    [SerializeField] MeshRenderer mesh;
    [SerializeField] ParticleSystem particle;
    [SerializeField] float explodeTime, exlosionRadius;

    [HideInInspector] public float damage;

    public virtual void Start() {
        StartCoroutine(Explode());
        mesh.material.DOColor(Color.red, .45f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    IEnumerator Explode() {
        yield return new WaitForSeconds(explodeTime);
        if(damage < 1) {
            damage = 1;
        }
        Instantiate(particle, transform.position, Quaternion.identity);
        InteractWithObjectInRadius();
        SoundManager.instance.PlaySound(SoundManager.instance.explosion);
        Destroy(gameObject);
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
            collider.GetComponent<EnemyBase>().TakeDamage(damage);
        } else if(collider.tag == "Door") {
            collider.GetComponent<DoorActions>().DestroyDoor();
        } else if(collider.tag == "SecretWall") {
            collider.GetComponent<SecretRoomWall>().DestroyWall();
        } else if(collider.tag == "Destructable") {
            Destroy(collider.gameObject);
        }
    }
}