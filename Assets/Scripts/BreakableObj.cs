using UnityEngine;

public class BreakableObj : MonoBehaviour
{
    private Health health;
    void Start()
    {
        health = GetComponent<Health>();
        health.onDeath.AddListener(onDeath);
    }

    void onDeath()
    {
        Destroy(gameObject);
    }
}
