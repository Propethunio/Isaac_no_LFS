using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] float untargetableTime = 1.5f;

    PlayerStats stats;
    List<SkinnedMeshRenderer> meshList;
    bool canBeDamaged = true;

    void Awake() {
        stats = GetComponent<PlayerStats>();
        meshList = new List<SkinnedMeshRenderer>(GetComponentsInChildren<SkinnedMeshRenderer>());
    }

    void Start() {
        LifeUIController.instance.SetHearts(stats.currHealth, stats.heartsContainers);
    }

    public bool CanBeHealed() {
        return stats.currHealth < stats.heartsContainers * 2;
    }

    public bool CanChangeHeartContainers() {
        return stats.heartsContainers < 8;
    }

    public void HealDamage(int amount) {
        for(int i = 0; i < amount; i++) {
            if(stats.currHealth < stats.heartsContainers * 2) {
                stats.currHealth++;
                LifeUIController.instance.HealDamage();
            }
        }
    }

    public void ChangeHeartsContainers(int amount, bool doesHeal) {
        for(int i = 0; i < amount; i++) {
            if(stats.heartsContainers < 8) {
                stats.heartsContainers++;
                LifeUIController.instance.AddHeart();
                if(doesHeal) {
                    StartCoroutine(HealInNextFrame());
                }
            }
        }
    }

    IEnumerator HealInNextFrame() {
        yield return new WaitForNextFrameUnit();
        HealDamage(2);
    }

    public void TakeDamage(int dmg) {
        if(!canBeDamaged) {
            return;
        }
        StartCoroutine(BecomeUntargetableForSpecificTime(untargetableTime));
        for(int i = 0; i < dmg; i++) {
            stats.currHealth--;
            LifeUIController.instance.TakeDamage();
        }
        if(stats.currHealth <= 0) {
            Die();
            return;
        }
        StartCoroutine(StartBlinking());
    }

    IEnumerator BecomeUntargetableForSpecificTime(float time) {
        canBeDamaged = false;
        yield return new WaitForSeconds(time);
        canBeDamaged = true;
    }

    IEnumerator StartBlinking() {
        while(!canBeDamaged) {
            meshList.ForEach(mesh => mesh.enabled = !mesh.enabled);
            yield return new WaitForSeconds(.2f);
        }
        meshList.ForEach(mesh => mesh.enabled = true);
    }

    void Die() {
        GameManager.instance.PlayerDied();
    }
}