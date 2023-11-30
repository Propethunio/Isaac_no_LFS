using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardController : MonoBehaviour {

    public static RewardController instance;

    [SerializeField] List<GameObject> powerUps;
    [SerializeField] LootTable roomLootTable, openChestLootTable, lockedChestLootTable, storeLootTable, barrelLootTable;
    [SerializeField] float easyRoomChanceOfPowerUp, mediumRoomChanceOfPowerUp, hardRoomChanceOfPowerUp, lockedChestChanceOfPowerUp, storeChanceOfPowerUp;
    [SerializeField] int minItemsFromOpenChest, maxItemsFromOpenChest, minItemsFromLockedChest, maxItemsFromLockedChest, minCollectableCost, maxCollectableCost, minPowerUpCost, maxPowerUpCost;

    public enum RoomRewardType {
        none,
        easy,
        medium,
        hard,
        boss
    }

    public enum ChestRewardType {
        unlocked,
        locked
    }


    public enum DestructableRewardType {
        none,
        barrel
    }

    void Awake() {
        instance = this;
    }

    public void CheckForShoopItem(out GameObject item, out int cost) {
        if(Random.value <= storeChanceOfPowerUp) {
            item = RollFromPowerUps();
            cost = Random.Range(minPowerUpCost, maxPowerUpCost + 1);
        } else {
            item = RollFromLootTable(storeLootTable);
            cost = Random.Range(minCollectableCost, maxCollectableCost + 1);
        }
    }

    public GameObject CheckForRoomReward(RoomRewardType rewardType) {
        switch(rewardType) {
            case RoomRewardType.easy:
                if(Random.value <= easyRoomChanceOfPowerUp) {
                    return RollFromPowerUps();
                }
                break;
            case RoomRewardType.medium:
                if(Random.value <= mediumRoomChanceOfPowerUp) {
                    return RollFromPowerUps();
                }
                break;
            case RoomRewardType.hard:
                if(Random.value <= hardRoomChanceOfPowerUp) {
                    return RollFromPowerUps();
                }
                break;
            case RoomRewardType.boss:
                return RollFromPowerUps();
        }
        return RollFromLootTable(roomLootTable);
    }

    GameObject RollFromLootTable(LootTable table) {
        float totalWeight = table.rewardList.Sum(item => item.weight);
        float roll = Random.Range(0, totalWeight);
        foreach(RewardItem item in table.rewardList) {
            if(item.weight >= roll) {
                return item.itemPrefab;
            }
            roll -= item.weight;
        }
        throw new System.Exception("Reward generation error: loot table");
    }

    public GameObject RollFromPowerUps() {
        int index = Random.Range(0, powerUps.Count);
        GameObject itemPrefab = powerUps[index];
        powerUps.RemoveAt(index);
        return itemPrefab;
    }

    public List<GameObject> CheckForChestReward(ChestRewardType rewardType) {
        List<GameObject> rewardsList = new List<GameObject>();
        switch(rewardType) {
            case ChestRewardType.unlocked:
                int itemsNum = Random.Range(minItemsFromOpenChest, maxItemsFromOpenChest + 1);
                for(int i = 0; i < itemsNum; i++) {
                    rewardsList.Add(RollFromLootTable(openChestLootTable));
                }
                return rewardsList;
            case ChestRewardType.locked:
                if(Random.value <= lockedChestChanceOfPowerUp) {
                    rewardsList.Add(RollFromPowerUps());
                    return rewardsList;
                }
                itemsNum = Random.Range(minItemsFromLockedChest, maxItemsFromLockedChest + 1);
                for(int i = 0; i < itemsNum; i++) {
                    rewardsList.Add(RollFromLootTable(lockedChestLootTable));
                }
                return rewardsList;

        }
        throw new System.Exception("Reward generation error: chest");
    }

    public GameObject CheckForDestructableReward(DestructableRewardType rewardType) {
        switch(rewardType) {
            case DestructableRewardType.barrel:
                return RollFromLootTable(barrelLootTable);
        }


        throw new System.Exception("Reward generation error: destructable");
    }
}