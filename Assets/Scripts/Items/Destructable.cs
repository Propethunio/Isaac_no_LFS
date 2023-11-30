using DG.Tweening;
using UnityEngine;

public class Destructable : MonoBehaviour {

    [SerializeField] RewardController.DestructableRewardType destructableRewardType;
    [SerializeField] ParticleSystem particle;
    [SerializeField] bool onlyBombCanDamage;
    [SerializeField] int health = 3;

    public void TakeDamage(int dmg) {
        if(!onlyBombCanDamage) {
            health -= dmg;
            if(health <= 0) {
                Destroy(gameObject);
            }
        }
    }

    void OnDestroy() {
        if(!gameObject.scene.isLoaded) {
            return;
        }
        for(int i = 0; i < 10; i++) {
            Instantiate(particle, transform.position, particle.transform.rotation);
        }
        if(destructableRewardType != RewardController.DestructableRewardType.none) {
            CheckForReward();
        }
    }

    void CheckForReward() {
        GameObject reward = RewardController.instance.CheckForDestructableReward(destructableRewardType);
        if(reward != null) {
            GameObject go = Instantiate(reward, new Vector3(transform.position.x, .25f, transform.position.z), reward.transform.rotation);
            Vector2 point = Random.insideUnitCircle.normalized * 2;
            go.transform.DOJump(new Vector3(transform.position.x + point.x, .25f, transform.position.z + point.y), 1, 1, 1.5f);
        }
    }
}