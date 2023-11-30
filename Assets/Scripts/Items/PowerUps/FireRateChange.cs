using UnityEngine;

public class FireRateChange : BasePowerUp {

    [SerializeField] float changeAmount, minStatAmount, maxStatAmount;

    public override void UseItemOnPlayer(GameObject player) {
        base.UseItemOnPlayer(player);
        PlayerStats stats = player.GetComponent<PlayerStats>();
        stats.fireRate += changeAmount;
        if(stats.fireRate < minStatAmount) {
            stats.fireRate = minStatAmount;
        }
        if(stats.fireRate > maxStatAmount) {
            stats.fireRate = maxStatAmount;
        }
    }
}