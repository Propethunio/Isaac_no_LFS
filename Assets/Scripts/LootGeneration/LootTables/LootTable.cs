using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RewardItem {

    public GameObject itemPrefab;
    public float weight;
}

[CreateAssetMenu(fileName = "LootTable")]
public class LootTable : ScriptableObject {

    public List<RewardItem> rewardList;
}