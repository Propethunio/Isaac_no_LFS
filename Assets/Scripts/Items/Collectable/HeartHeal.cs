using UnityEngine;

public class HeartHeal : BaseItem {

    [SerializeField] int healAmount;

    public override void UseItemOnPlayer(GameObject player) {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if(playerHealth.CanBeHealed()) {
            playerHealth.HealDamage(healAmount);
            SoundManager.instance.PlaySound(SoundManager.instance.healthPickup);
            Destroy(gameObject);
        }
    }
}