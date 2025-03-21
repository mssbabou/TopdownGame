using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public LayerMask ItemLayer;

    public List<ItemType> Inventory;

    public float PickupRange;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, PickupRange, ItemLayer);

        foreach (Collider2D hit in hits) 
        {
            ItemObject itemObject = hit.GetComponent<ItemObject>();

            if (itemObject != null)
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
    }
}

public enum ItemType
{
    RedKeycard,
    GreenKeycard,
    BlueKeycard,
}
