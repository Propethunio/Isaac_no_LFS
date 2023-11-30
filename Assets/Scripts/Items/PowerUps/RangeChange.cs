using UnityEngine;

public class RangeChange : BasePowerUp {

    [SerializeField] float changeAmount, minStatAmount, maxStatAmount;

    public override void UseItemOnPlayer(GameObject player) {
        base.UseItemOnPlayer(player);
        PlayerStats stats = player.GetComponent<PlayerStats>();
        stats.fireRange += changeAmount;
        if(stats.fireRange < minStatAmount) {
            stats.fireRange = minStatAmount;
        }
        if(stats.fireRange > maxStatAmount) {
            stats.fireRange = maxStatAmount;
        }
    }
}