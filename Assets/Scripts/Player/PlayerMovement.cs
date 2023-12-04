using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    const string ISWALKING = "isWalking";
    const string HORIZONTAL = "x";
    const string VERTICAL = "y";

    PlayerInput input;
    PlayerStats stats;
    CharacterController controller;
    Animator anim;
    Vector3 moveDir;

    void Awake() {
        input = GetComponent<PlayerInput>();
        stats = GetComponent<PlayerStats>();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update() {
        HandleMovementInput();
    }

    void FixedUpdate() {
        HandleMovement();
    }

    void HandleMovementInput() {
        Vector2 moveInputVector = input.GetMovementVectorNormalized();
        if(moveInputVector == Vector2.zero) {
            moveDir = Vector3.zero;
            anim.SetBool(ISWALKING, false);
            return;
        }
        moveDir = new Vector3(moveInputVector.x, 0f, moveInputVector.y);
        Vector2 shootInputVector = input.GetShootingVectorNormalized();
        if(shootInputVector == Vector2.zero) {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, stats.rotateSpeed * Time.deltaTime);
        }
        HandleAnim();
    }

    void HandleAnim() {
        anim.SetBool(ISWALKING, true);
        float rotationAngle = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);
        Vector3 moveVector = Quaternion.AngleAxis(rotationAngle, Vector3.down) * moveDir;
        anim.SetFloat(HORIZONTAL, moveVector.x);
        anim.SetFloat(VERTICAL, moveVector.z);
    }

    void HandleMovement() {
        if(controller.enabled) {
            controller.Move(moveDir * stats.moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void MoveToPosition(Vector3 pos) {
        controller.enabled = false;
        transform.position = pos;
        controller.enabled = true;
    }

    public void TweenToPosition(Vector3 pos) {
        controller.enabled = false;
        StartCoroutine(TweenTo(pos));
    }

    IEnumerator TweenTo(Vector3 pos) {
        transform.DOMove(new Vector3(pos.x, 0, pos.z), .25f);
        yield return new WaitForSeconds(.25f);
        controller.enabled = true;
    }
}