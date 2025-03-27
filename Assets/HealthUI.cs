using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public Health health;

    public TextMeshProUGUI HealthText;

    void Start()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health == null) return;

        HealthText.text = health.CurrentHealth.ToString();
    }
}
