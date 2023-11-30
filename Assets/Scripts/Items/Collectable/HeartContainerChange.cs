using UnityEngine;

public class HeartContainerChange : BaseItem {

    [SerializeField] int changeAmount;
    [SerializeField] bool doesHeal;

    public override void UseItemOnPlayer(GameObject player) {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if(playerHealth.CanChangeHeartContainers()) {
            playerHealth.ChangeHeartsContainers(changeAmount, doesHeal);
            SoundManager.instance.PlaySound(SoundManager.instance.healthPickup);
            Destroy(gameObject);
        }
    }
}