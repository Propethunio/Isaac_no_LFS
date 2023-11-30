using UnityEngine;

public class Spikes : MonoBehaviour {

    void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Player") {
            other.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
}