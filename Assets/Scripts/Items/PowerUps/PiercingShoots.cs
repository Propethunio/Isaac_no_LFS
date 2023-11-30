using UnityEngine;

public class PiercingShoots : BasePowerUp {

    [SerializeField] bool piercingShoots;

    public override void UseItemOnPlayer(GameObject player) {
        base.UseItemOnPlayer(player);
        PlayerStats stats = player.GetComponent<PlayerStats>();
        stats.piercingShoots = piercingShoots;
    }
}