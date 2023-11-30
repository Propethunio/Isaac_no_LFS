using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    public AudioClip[] explosion;
    public AudioClip[] shoot;
    public AudioClip[] useKey;
    public AudioClip[] doorClose;
    public AudioClip[] doorOpen;
    public AudioClip[] chestOpen;
    public AudioClip[] powerUp;
    public AudioClip[] itemPickup;
    public AudioClip[] healthPickup;

    Transform CameraTransform;
    float volume = .5f;

    void Awake() {
        instance = this;
    }

    void Start() {
        CameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    public void PlaySound(AudioClip[] audioClipArray) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)]);
    }

    void PlaySound(AudioClip audioClip) {
        AudioSource.PlayClipAtPoint(audioClip, CameraTransform.position, volume);
    }
}