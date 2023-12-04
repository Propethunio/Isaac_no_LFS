using TMPro;
using UnityEngine;

public class StoreItem : MonoBehaviour {

    [SerializeField] TextMeshProUGUI costText;

    public BaseItem item;
    public int cost;

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if(playerStats.coinsAmount >= cost) {
                playerStats.coinsAmount -= cost;
                item.UseItemOnPlayer(other.gameObject);
                ItemsUIController.instance.SetCoins(playerStats.coinsAmount);
                StatisticManager.instance.coinsCount += cost;
                Destroy(gameObject);
            }
        }
    }

    public void SetCost() {
        costText.text = cost.ToString();
    }
}