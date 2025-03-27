using System;
using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public string weaponName;
    public int damage;

    public Transform Crowbar;
    private SpriteRenderer spriteRenderer;
    private bool canSwing = true;



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Hit()
    {
        StartCoroutine(SwingWeapon());
    }

    private IEnumerator SwingWeapon()
    {
        if (!canSwing) yield break;

        canSwing = false;

        float elapsedTime = 0f;
        float duration = 0.5f;
        spriteRenderer.enabled = true;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Crowbar.Rotate(0, 0, 360 * Time.deltaTime / duration);
            yield return null;
        }

        spriteRenderer.enabled = false;

        Crowbar.localRotation = Quaternion.identity;

        yield return new WaitForSeconds(0.1f);
        canSwing = true;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();
        if (health != null)
        {
            // Deal damage to the object
            health.TakeDamage(damage);
        }
    }
}