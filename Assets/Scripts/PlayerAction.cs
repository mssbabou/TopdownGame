using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public GameObject SelectUI;
    public SpriteRenderer PlayerSprite;
    public Color NormalColor = Color.white;
    public Color DamagedColor = Color.red;
    public float DamagedColorTime = 0.1f;

    public LayerMask ItemLayer;

    public List<ItemType> Inventory;

    public float PickupRange = 0.8f;

    public float InteractRadius = 0.5f;
    public Vector2 InteractOffset = Vector2.up;

    private Health health;

    void Start()
    {
        SelectUI.SetActive(false);
        health = GetComponent<Health>();
        health.onTakeDamage.AddListener(TakeDamage);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPickup();

        UpdateInteractions();
    }

    private void UpdateInteractions()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + transform.TransformDirection(InteractOffset), InteractRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.TryGetComponent<Interactable>(out var interactable))
            {
                SelectUI.SetActive(true);
                SelectUI.transform.position = hit.transform.position;
                SelectUI.transform.rotation = Quaternion.identity;

                if (Input.GetKeyDown(KeyCode.E)) interactable.Interact(this);

                return;
            }
        }

        SelectUI.SetActive(false);
    }

    private void CheckPickup()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, PickupRange, ItemLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<ItemObject>(out var itemObject))
            {
                Inventory.Add(itemObject.Type);
                Destroy(hit.gameObject);
            }
        }
    }

    private void TakeDamage()
    {
        StartCoroutine(TakeDamageColorChange());
    }

    private IEnumerator TakeDamageColorChange()
    {
        PlayerSprite.color = DamagedColor;
        yield return new WaitForSeconds(DamagedColorTime);
        PlayerSprite.color = NormalColor;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PickupRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere (transform.position + transform.TransformDirection(InteractOffset), InteractRadius);
    }
}

public enum ItemType
{
    RedKeycard,
    GreenKeycard,
    BlueKeycard,
    FlashLight,
}
