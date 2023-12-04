using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider MusicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;

    public void SetMasterVolume(float value) {
        SoundManager.instance.masterVolume = value / 100;
        SoundManager.instance.UpdateMusicVolume();
    }

    public void SetMusicVolume(float value) {
        SoundManager.instance.musicVolume = value / 100;
        SoundManager.instance.UpdateMusicVolume();
    }

    public void SetSfxVolume(float value) {
        SoundManager.instance.sfxVolume = value / 100;
    }
}