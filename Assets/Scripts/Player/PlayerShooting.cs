using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] Transform firePoint;

    const string SHOOT = "shoot";

    PlayerInput input;
    PlayerStats stats;
    Animator anim;
    float lastShoot;

    void Start() {
        input = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
    }

    void Update() {
        HandleShootingInput();
    }

    void HandleShootingInput() {
        Vector2 shootInputVector = input.GetShootingVectorNormalized();
        if(shootInputVector != Vector2.zero) {
            HandleShooting(shootInputVector);
        }
    }

    void HandleShooting(Vector2 shootInputVector) {
        Vector3 shootingDir = new Vector3(shootInputVector.x, 0f, shootInputVector.y);
        transform.forward = Vector3.Slerp(transform.forward, shootingDir, stats.rotateSpeed * Time.deltaTime);
        float rotationAngle = Mathf.Abs(Vector3.SignedAngle(shootingDir, transform.forward, Vector3.up));
        if(rotationAngle < 25f) {
            Shoot();
        }
    }

    void Shoot() {
        if(Time.time >= 1f / stats.fireRate + lastShoot) {
            Instantiate(muzzleFlash, firePoint.position, firePoint.rotation, firePoint);
            anim.SetTrigger(SHOOT);
            SoundManager.instance.PlaySound(SoundManager.instance.shoot);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.range = stats.fireRange;
            bulletScript.piercing = stats.piercingShoots;
            bulletScript.damage = stats.damage;
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * stats.fireForce, ForceMode.Impulse);
            lastShoot = Time.time;
        }
    }
}