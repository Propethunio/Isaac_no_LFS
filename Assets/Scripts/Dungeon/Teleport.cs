using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour {

    [SerializeField] ParticleSystem particle;

    new CapsuleCollider collider;
    Room room;
    bool isTeleporting;

    void Start() {
        collider = GetComponent<CapsuleCollider>();
        collider.enabled = false;
        room = GetComponentInParent<Room>();
    }

    public void StartCollide() {
        StartCoroutine(StartColliding());
    }

    IEnumerator StartColliding() {
        yield return new WaitForSeconds(2f);
        collider.enabled = true;
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player" && !isTeleporting && !room.DoesHaveEnemies()) {
            isTeleporting = true;
            GameManager.instance.TeleportPlayer(transform.position);
            Instantiate(particle, new Vector3(transform.position.x, 1f, transform.position.z), Quaternion.identity, transform);
        }
    }
}