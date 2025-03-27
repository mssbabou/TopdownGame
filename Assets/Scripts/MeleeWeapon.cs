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
    private Collider2D collider2D;
    private bool canSwing = true;



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
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
        collider2D.enabled = true;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Crowbar.Rotate(0, 0, 360 * Time.deltaTime / duration);
            yield return null;
        }

        spriteRenderer.enabled = false;
        collider2D.enabled = false;

        Crowbar.localRotation = Quaternion.identity;

        yield return new WaitForSeconds(0.1f);
        canSwing = true;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();
        Door door = collision.GetComponentInParent<Door>();

        if (health != null)
        {
            Debug.Log($"Hit {collision.gameObject.name} or its parent for {damage} damage.");
            health.TakeDamage(damage);
        }
        if (door != null)
        {
            Debug.Log($"Hit {collision.gameObject.name} (a door).");
            door.TakeDamage(damage);
        }
        else
        {
            Debug.Log($"Hit {collision.gameObject.name} but neither it nor its parent has health.");
        }
    }
}