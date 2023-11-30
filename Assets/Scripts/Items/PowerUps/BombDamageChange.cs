using UnityEngine;

public class BombDamageChange : BasePowerUp {

    [SerializeField] float changeAmount, minStatAmount, maxStatAmount;

    public override void UseItemOnPlayer(GameObject player) {
        base.UseItemOnPlayer(player);
        PlayerStats stats = player.GetComponent<PlayerStats>();
        stats.bombDamageMultiplier += changeAmount;
        if(stats.bombDamageMultiplier < minStatAmount) {
            stats.bombDamageMultiplier = minStatAmount;
        }
        if(stats.bombDamageMultiplier > maxStatAmount) {
            stats.bombDamageMultiplier = maxStatAmount;
        }
    }
}