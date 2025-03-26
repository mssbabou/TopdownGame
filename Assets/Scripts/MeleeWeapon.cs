using System.Collections;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public string weaponName;
    public int damage;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>(); // Ensure the Animator is on the character
    }

    public void Hit()
    {
        animator.Play("WeaponHit");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has a Health component
        Health health = collision.GetComponent<Health>();
        if (health != null)
        {
            // Deal damage to the object
            health.TakeDamage(damage);
        }
    }
}