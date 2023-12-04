using UnityEngine;
using UnityEngine.AI;

public class TrollBomb : Bomb {

    GameObject player;
    NavMeshAgent agent;

    public override void Start() {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        player = GameManager.instance.GetPlayer();
    }

    void Update() {
        agent.destination = player.transform.position;
    }
}