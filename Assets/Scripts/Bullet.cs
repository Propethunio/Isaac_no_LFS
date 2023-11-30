using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float range;
    public float damage;
    public bool piercing;

    Vector3 startPos;
    List<Collider> colliders = new List<Collider>();

    void Start() {
        startPos = transform.position;
    }

    void Update() {
        CheckIfOutOfRange();
    }

    void CheckIfOutOfRange() {
        float dis = Vector3.Distance(startPos, transform.position);
        if(dis >= range) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(colliders.Contains(other)) {
            return;
        }
        colliders.Add(other);
        if(other.tag == "Bullet" || other.tag == "Item") {
            return;
        }
        if(other.tag == "Player") {
            other.GetComponent<PlayerHealth>().TakeDamage(1);
            if(piercing) {
                return;
            }
        }
        if(other.tag == "Enemy") {
            other.GetComponent<EnemyBase>().TakeDamage(damage);
            if(piercing) {
                return;
            }
        }
        if(other.tag == "Destructable") {
            other.GetComponent<Destructable>().TakeDamage(1);
        }
        Destroy(gameObject);
    }
}