using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCallback : MonoBehaviour {

    void OnParticleSystemStopped() {
        GameManager.instance.PlayerWin();
    }
}