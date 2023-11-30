using UnityEngine;

public class Key : BaseItem {

    public override void UseItemOnPlayer(GameObject player) {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.keysAmount += 1;
        ItemsUIController.instance.SetKeys(playerStats.keysAmount);
        SoundManager.instance.PlaySound(SoundManager.instance.itemPickup);
        Destroy(gameObject);
    }
}