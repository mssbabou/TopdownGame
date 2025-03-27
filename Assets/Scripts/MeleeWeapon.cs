using System;
using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public string weaponName;
    public int damage;

    public SpriteRenderer spi;



    void Start()
    {

    }

    public void Hit()
    {
        // Move the weapon forward and swing
        StartCoroutine(SwingWeapon());
    }

    private IEnumerator SwingWeapon()
    {
        spi.enabled = true;
        // Move the weapon forward
        Vector3 originalPosition = transform.localPosition;
        Vector3 targetPosition = originalPosition + Vector3.forward * 1.5f;
        float swingSpeed = 3f;
        float progress = 0f;
        while (progress <= 1)
        {
            transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, progress);
            progress += Time.deltaTime * swingSpeed;
            yield return null;
        }
        // Move the weapon back
        progress = 0f;
        while (progress <= 1)
        {
            transform.localPosition = Vector3.Lerp(targetPosition, originalPosition, progress);
            progress += Time.deltaTime * swingSpeed;
            yield return null;
        }
        transform.localPosition = originalPosition;
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