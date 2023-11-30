using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUseBomb : MonoBehaviour {

    [SerializeField] GameObject bombPrefab;

    PlayerStats stats;

    void Awake() {
        stats = GetComponent<PlayerStats>();
    }

    public void TryUseBomb(InputAction.CallbackContext context) {
        if(context.performed && stats.bombsAmount > 0) {
            UseBomb();
        }
    }

    void UseBomb() {
        stats.bombsAmount--;
        ItemsUIController.instance.SetBombs(stats.bombsAmount);
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        bomb.GetComponent<Bomb>().damage = stats.moveSpeed * stats.bombDamageMultiplier;
    }
}