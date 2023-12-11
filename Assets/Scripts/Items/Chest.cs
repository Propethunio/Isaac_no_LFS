using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    [SerializeField] RewardController.ChestRewardType chestRewardType;
    [SerializeField] List<GameObject> modelsToShow, modelsToHide;
    [SerializeField] BoxCollider trigger;
    [SerializeField] int keyCost;

    void Start() {
        trigger.enabled = false;
        StartCoroutine(StartColliding());
    }
    IEnumerator StartColliding() {
        yield return new WaitForSeconds(1.25f);
        trigger.enabled = true;
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if(playerStats.keysAmount >= keyCost) {
                trigger.enabled = false;
                playerStats.keysAmount -= keyCost;
                ItemsUIController.instance.SetKeys(playerStats.keysAmount);
                OpenChest();
            }
        }
    }

    void OpenChest() {
        SoundManager.instance.PlaySound(SoundManager.instance.chestOpen);
        if(keyCost > 0) {
            SoundManager.instance.PlaySound(SoundManager.instance.useKey);
        }
        modelsToShow.ForEach(model => { model.SetActive(true); });
        modelsToHide.ForEach(model => { model.SetActive(false); });
        List<GameObject> rewardPrefabs = RewardController.instance.CheckForChestReward(chestRewardType);
        rewardPrefabs.ForEach(prefab => {
            int tries = 0;
            Vector2 point = Random.insideUnitCircle.normalized * 2;
            while(Physics.CheckSphere(new Vector3(transform.position.x + point.x, .25f, transform.position.z + point.y), .35f, ~(1 << 6))) {
                point = Random.insideUnitCircle.normalized * 2;
                tries++;
                if(tries > 100) {
                    break;
                }
            }
            if(tries <= 100) {
                GameObject go = Instantiate(prefab, new Vector3(transform.position.x, .25f, transform.position.z), prefab.transform.rotation);
                go.transform.DOJump(new Vector3(transform.position.x + point.x, .25f, transform.position.z + point.y), 1, 1, 1.5f);
            }
        });
    }
}