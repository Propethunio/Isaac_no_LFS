using UnityEngine;

public class BombUnlit : BaseItem {

    public override void UseItemOnPlayer(GameObject player) {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.bombsAmount += 1;
        ItemsUIController.instance.SetBombs(playerStats.bombsAmount);
        SoundManager.instance.PlaySound(SoundManager.instance.itemPickup);
        Destroy(gameObject);
    }
}