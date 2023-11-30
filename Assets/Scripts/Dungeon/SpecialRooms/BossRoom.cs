using System.Collections;

public class BossRoom : Room {

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
        bool bossAlive = true;
        float maxHp = GetEnemiesMaxHealth();
        float lastFramecurrHp = 0f;
        while(bossAlive) {
            float currHp = GetEnemiesCurrentHealth();
            if(currHp != lastFramecurrHp) {
                if(currHp <= 0f) {
                    bossAlive = false;
                }
                lastFramecurrHp = currHp;
                BossHealthUIController.instance.UpdateBossHealth(currHp / maxHp);
            }
            yield return null;
        }
        BossHealthUIController.instance.EndBossFight();
    }
}