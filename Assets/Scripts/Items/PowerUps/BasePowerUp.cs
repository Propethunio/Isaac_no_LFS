using DG.Tweening;
using UnityEngine;

public class BasePowerUp : BaseItem {

    new void Start() {
        base.Start();
        transform.DORotate(new Vector3(0, 360, 0), 6, RotateMode.FastBeyond360).SetRelative().SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public override void UseItemOnPlayer(GameObject player) {
        Destroy(gameObject);
        SoundManager.instance.PlaySound(SoundManager.instance.powerUp);
    }
}