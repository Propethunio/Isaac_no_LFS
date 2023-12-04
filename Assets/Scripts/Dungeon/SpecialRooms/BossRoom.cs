using System.Collections;
using UnityEngine;

public class BossRoom : Room {

    [SerializeField] GameObject teleport;
    [SerializeField] Transform rewardPosition;

    public override void Start() {
        base.Start();
        teleport.SetActive(false);
    }

    public override void StartFight() {
        base.StartFight();
        StartBossFight();
    }

    public float GetEnemiesMaxHealth() {
        float max = 0f;
        enemiesList.ForEach(enemy => max += enemy.health);
        return max;
    }

    public float GetEnemiesCurrentHealth() {
        float curr = 0f;
        enemiesList.ForEach(enemy => curr += enemy.hp);
        return curr;
    }

    public void StartBossFight() {
        BossHealthUIController.instance.StartBossFight();
        StartCoroutine(UpdateBossHealthSlider());
    }

    IEnumerator UpdateBossHealthSlider() {
        yield return null;
        bool bossAlive = true;
        float maxHp = GetEnemiesMaxHealth();
        float lastFrameCurrHp = GetEnemiesCurrentHealth();
        while(bossAlive) {
            float currHp = GetEnemiesCurrentHealth();
            if(currHp != lastFrameCurrHp) {
                if(currHp <= 0f) {
                    bossAlive = false;
                }
                lastFrameCurrHp = currHp;
                BossHealthUIController.instance.UpdateBossHealth(currHp / maxHp);
            }
            yield return null;
        }
        BossHealthUIController.instance.EndBossFight();
    }

    public override void SpawnReward() {
        GameObject reward = RewardController.instance.CheckForRoomReward(roomRewardType);
        Instantiate(reward, rewardPosition);
        teleport.SetActive(true);
        teleport.GetComponent<Teleport>().StartCollide();
    }
}