using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public LayerMask ItemLayer;

    public List<ItemType> Inventory;

    public float PickupRange = 0.8f;

    public float InteractRadius = 0.5f;
    public Vector2 InteractOffset = Vector2.up;

    // Update is called once per frame
    void Update()
    {
        CheckPickup();

        if (Input.GetKeyDown(KeyCode.E))
            CheckInteractions();
    }

    private void CheckInteractions()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + transform.TransformDirection(InteractOffset), InteractRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.TryGetComponent<Interactable>(out var interactable))
            {
                interactable.Interact(this);
            }
        }
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
}
