using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

    GameObject player;
    NavMeshAgent agent;
    Animator anim;

    const string ISWALKING = "isWalking", ATTACK = "attack";

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameManager.instance.GetPlayer();
        agent.updateRotation = false;
        agent.enabled = true;
    }

    void Update() {
        HandleMovement();
        HandleAnim();
    }

    void HandleMovement() {
        agent.destination = player.transform.position;
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
    }

    void HandleAnim() {
        if(agent.velocity != Vector3.zero) {
            anim.SetBool(ISWALKING, true);
        } else {
            anim.SetBool(ISWALKING, false);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject == player) {
            anim.SetTrigger(ATTACK);
        }
    }
}