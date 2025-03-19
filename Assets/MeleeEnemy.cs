using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private Health health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<Health>();

        health.onDeath.AddListener(Die);
    }

    private void Die()
    {
        health.onDeath.RemoveAllListeners();

        // Play Death Animation


        // Despawn
        Destroy(gameObject);
    }
}
