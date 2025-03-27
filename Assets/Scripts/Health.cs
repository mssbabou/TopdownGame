using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float MaxHealth = 100f;
    public float CurrentHealth;

    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent onTakeDamage;
    public UnityEvent onHeal;

    // Start is called before the first frame update
    private void Start()
    {
        // Initialize current health to max health at the start
        CurrentHealth = MaxHealth;
    }

    // Take damage function
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth); // Ensure health doesn't go below 0

        // Trigger health change event
        onTakeDamage.Invoke();
        // Check if health reaches zero and trigger death event
        if (CurrentHealth == 0)
        {
            onDeath.Invoke();
        }
    }

    // Heal function
    public void Heal(float amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth); // Ensure health doesn't exceed maxHealth

        // Trigger health change event
        onHeal.Invoke();
    }

    // For debugging purposes (displaying health in the Inspector)
    private void OnGUI()
    {
        //GUILayout.Label(gameObject.name + " Health: " + CurrentHealth + "/" + MaxHealth);
    }
}
