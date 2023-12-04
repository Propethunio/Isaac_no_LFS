using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;
    public AudioClip endMusic;
    public AudioClip[] explosion;
    public AudioClip[] shoot;
    public AudioClip[] useKey;
    public AudioClip[] doorClose;
    public AudioClip[] doorOpen;
    public AudioClip[] chestOpen;
    public AudioClip[] powerUp;
    public AudioClip[] itemPickup;
    public AudioClip[] healthPickup;

    [HideInInspector] public float masterVolume = 1f, musicVolume = 1f, sfxVolume = 1f;

    void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip[] audioClipArray, float volume) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], volume);
    }

    public void PlaySound(AudioClip[] audioClipArray) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], 1f);
    }

    void PlaySound(AudioClip audioClip, float volume) {
        sfxSource.PlayOneShot(audioClip, masterVolume * sfxVolume * volume);
    }

    public void UpdateMusicVolume() {
        musicSource.volume = masterVolume * musicVolume;
    }
}