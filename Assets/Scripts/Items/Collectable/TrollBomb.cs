using UnityEngine;
using UnityEngine.AI;

public class TrollBomb : Bomb {

    GameObject player;
    NavMeshAgent agent;

    public override void Start() {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        player = GameController.instance.GetPlayer();
    }

    void Update() {
        agent.destination = player.transform.position;
    }
}