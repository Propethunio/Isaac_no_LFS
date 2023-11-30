using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BaseItem : MonoBehaviour {

    [HideInInspector] public new BoxCollider collider;

    void Awake() {
        collider = GetComponent<BoxCollider>();
    }

    public void Start() {
        StartCoroutine(StartColliding());
    }

    void OnDestroy() {
        DOTween.Kill(transform);
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            UseItemOnPlayer(other.gameObject);
        }
    }

    IEnumerator StartColliding() {
        yield return new WaitForSeconds(1.25f);
        if(collider) {
            collider.enabled = true;
        }
        transform.DOMoveY(.4f, 1.15f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public virtual void UseItemOnPlayer(GameObject player) { }
}