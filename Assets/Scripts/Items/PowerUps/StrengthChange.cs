using UnityEngine;

public class StrengthChange : BasePowerUp {

    [SerializeField] float changeAmount, minStatAmount, maxStatAmount;

    public override void UseItemOnPlayer(GameObject player) {
        base.UseItemOnPlayer(player);
        PlayerStats stats = player.GetComponent<PlayerStats>();
        stats.damage += changeAmount;
        if(stats.damage < minStatAmount) {
            stats.damage = minStatAmount;
        }
        if(stats.damage > maxStatAmount) {
            stats.damage = maxStatAmount;
        }
    }
}