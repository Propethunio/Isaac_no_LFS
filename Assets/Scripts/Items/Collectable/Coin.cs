using UnityEngine;

public class Coin : BaseItem {

    public override void UseItemOnPlayer(GameObject player) {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.coinsAmount += 1;
        ItemsUIController.instance.SetCoins(playerStats.coinsAmount);
        SoundManager.instance.PlaySound(SoundManager.instance.itemPickup);
        Destroy(gameObject);
    }
}