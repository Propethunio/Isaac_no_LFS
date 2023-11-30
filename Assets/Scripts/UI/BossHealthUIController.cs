using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUIController : MonoBehaviour {

    public static BossHealthUIController instance;

    [SerializeField] Slider healthSlider;
    [SerializeField] Image fillArea;
    [SerializeField] Color standardColor;
    [SerializeField] Color flashColor;

    void Awake() {
        instance = this;
    }

    public void StartBossFight() {
        healthSlider.value = 1f;
        healthSlider.gameObject.SetActive(true);
    }

    public void EndBossFight() {
        healthSlider.gameObject.SetActive(false);
    }

    public void UpdateBossHealth(float value) {
        healthSlider.value = value;
        StartCoroutine(FlashSliderEffect());
    }

    IEnumerator FlashSliderEffect() {
        fillArea.color = flashColor;
        yield return new WaitForSeconds(.1f);
        fillArea.color = standardColor;
    }
}