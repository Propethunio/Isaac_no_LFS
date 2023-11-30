using UnityEngine;

public class SpeedChange : BasePowerUp {

    [SerializeField] float changeAmount, minStatAmount, maxStatAmount;

    public override void UseItemOnPlayer(GameObject player) {
        base.UseItemOnPlayer(player);
        PlayerStats stats = player.GetComponent<PlayerStats>();
        stats.moveSpeed += changeAmount;
        if(stats.moveSpeed < minStatAmount) {
            stats.moveSpeed = minStatAmount;
        }
        if(stats.moveSpeed > maxStatAmount) {
            stats.moveSpeed = maxStatAmount;
        }
    }
}