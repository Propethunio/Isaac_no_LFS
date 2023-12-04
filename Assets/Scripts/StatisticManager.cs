using TMPro;
using UnityEngine;

public class StatisticManager : MonoBehaviour {

    public static StatisticManager instance;

    [SerializeField] TMP_Text enemiesText;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text bombsText;

    [HideInInspector] public int enemiesCount, coinsCount, bombsCount;

    void Awake() {
        instance = this;
    }

    public void SetStats() {
        enemiesText.text = enemiesCount.ToString();
        coinsText.text = coinsCount.ToString();
        bombsText.text = bombsCount.ToString();
    }
}