using TMPro;
using UnityEngine;

public class ItemsUIController : MonoBehaviour {

    public static ItemsUIController instance;

    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text keysText;
    [SerializeField] TMP_Text bombsText;

    void Awake() {
        instance = this;
    }

    public void SetCoins(int amount) {
        coinsText.text = amount.ToString();
    }

    public void SetKeys(int amount) {
        keysText.text = amount.ToString();
    }

    public void SetBombs(int amount) {
        bombsText.text = amount.ToString();
    }
}