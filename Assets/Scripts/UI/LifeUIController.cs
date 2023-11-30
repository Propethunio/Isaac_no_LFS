using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUIController : MonoBehaviour {

    public static LifeUIController instance;

    [SerializeField] GameObject lifeHeartPrefab;

    int currLife;
    int heartsAmount;
    List<HeartUI> hearts = new List<HeartUI>();

    void Awake() {
        instance = this;
    }

    public void SetHearts(int currHealth, int hearts) {
        heartsAmount = hearts;
        for(int i = 0; i < heartsAmount; i++) {
            Instantiate(lifeHeartPrefab, transform);
        }
        StartCoroutine(SetFillAmount(currHealth));
    }

    IEnumerator SetFillAmount(int amountToFill) {
        while(hearts.Count != heartsAmount) {
            yield return null;
        }
        for(int i = 0; i < amountToFill; i++) {
            HealDamage();
        }
    }

    public void RegisterHeart(HeartUI heart) {
        hearts.Add(heart);
    }

    public void TakeDamage() {
        if(currLife < 1) {
            return;
        }
        int heartIndex = Mathf.CeilToInt((float)currLife / 2) - 1;
        hearts[heartIndex].TakeDamage();
        currLife--;
    }

    public void HealDamage() {
        if(currLife >= heartsAmount * 2) {
            return;
        }
        int heartIndex = Mathf.CeilToInt(((float)currLife + 1) / 2) - 1;
        hearts[heartIndex].Heal();
        currLife++;
    }

    public void AddHeart() {
        StartCoroutine(AddHeartsCoroutine());
    }

    IEnumerator AddHeartsCoroutine() {
        Instantiate(lifeHeartPrefab, transform);
        heartsAmount++;
        while(hearts.Count != heartsAmount) {
            yield return null;
        }
    }

    public void RemoveHeart() {
        if(heartsAmount <= 1) {
            return;
        }
        int indexToRemove = hearts.Count - 1;
        GameObject heart = hearts[indexToRemove].gameObject;
        hearts.RemoveAt(indexToRemove);
        Destroy(heart);
        heartsAmount--;
        if(currLife > heartsAmount * 2) {
            currLife = heartsAmount * 2;
        }
    }
}