using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Minotaur : MonoBehaviour {

    [SerializeField] float chargeSpeed, chargeChance, damageOnPilarHit;

    Animator anim;
    GameObject player;
    Rigidbody rb;
    NavMeshAgent agent;
    Coroutine coroutine;

    bool isCharging, updateRotation;

    void Start() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = GameManager.instance.GetPlayer();
        agent.enabled = true;
        coroutine = StartCoroutine(ChooseAttack());
    }

    void Update() {
        if(agent.enabled) {
            HandleMovement();
        }
    }

    void HandleMovement() {
        agent.destination = player.transform.position;
        if(agent.velocity != Vector3.zero) {
            anim.SetBool("walk", true);
        } else {
            anim.SetBool("walk", false);
        }
    }

    IEnumerator ChooseAttack() {
        float extraChance = 0;
        while(true) {
            yield return new WaitForSeconds(Random.Range(2.5f, 5f));
            float random = Random.value;
            if(random < chargeChance + extraChance) {
                Charge();
            }
            extraChance += .15f;
        }
    }

    void Charge() {
        StopCoroutine(coroutine);
        anim.SetBool("walk", false);
        anim.SetTrigger("charge");
        updateRotation = true;
        agent.enabled = false;
        StartCoroutine(LookAtPlayer());
    }

    IEnumerator LookAtPlayer() {
        while(updateRotation) {
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            yield return null;
        }
    }

    void Run() {
        isCharging = true;
        updateRotation = false;
        rb.AddForce(new Vector3(player.transform.position.x - transform.position.x, 0f, player.transform.position.z - transform.position.z).normalized * chargeSpeed, ForceMode.Impulse);
    }

    void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Bullet") {
            return;
        }
        if(other.gameObject.tag == "Player" && !isCharging) {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(ChooseAttack());
            anim.SetTrigger("attack");
            return;
        }
        if(other.gameObject.tag != "Player" && isCharging) {
            if(other.tag == "Destructable") {
                Destroy(other.gameObject);
                gameObject.GetComponent<EnemyBase>().TakeDamage(damageOnPilarHit);
            }
            EndCharge();
        }
    }

    void EndCharge() {
        isCharging = false;
        rb.velocity = Vector3.zero;
        anim.SetTrigger("endCharge");
    }

    IEnumerator EndStun() {
        yield return new WaitForSeconds(.4f);
        agent.enabled = true;
        coroutine = StartCoroutine(ChooseAttack());
    }
}