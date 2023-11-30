using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour {

    [SerializeField] Image fillHeart;

    void Start() {
        LifeUIController.instance.RegisterHeart(this);
    }

    public void TakeDamage() {
        fillHeart.fillAmount -= .5f;
    }

    public void Heal() {
        fillHeart.fillAmount += .5f;
    }
}