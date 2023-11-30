using System.Collections.Generic;
using UnityEngine;

public class StoreRoom : MonoBehaviour {

    [SerializeField] GameObject storeItemPrefab;
    [SerializeField] List<Transform> itemSpots;

    void Start() {
        GameObject item;
        int cost;
        itemSpots.ForEach(spot => {
            RewardController.instance.CheckForShoopItem(out item, out cost);
            StoreItem storeItem = Instantiate(storeItemPrefab, spot).GetComponent<StoreItem>();
            BaseItem itemPrefab = Instantiate(item, storeItem.transform).GetComponent<BaseItem>();
            storeItem.item = itemPrefab;
            storeItem.cost = cost;
            storeItem.SetCost();
            Destroy(itemPrefab.collider);
        });
    }
}