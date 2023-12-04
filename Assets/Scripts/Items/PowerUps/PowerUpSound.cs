using UnityEngine;

public class PowerUpSound : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        SoundManager.instance.PlaySound(SoundManager.instance.powerUp);
    }
}