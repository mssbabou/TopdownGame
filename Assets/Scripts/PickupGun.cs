using Unity.VisualScripting;
using UnityEngine;

public class PickupGun : MonoBehaviour
{
    public float PickupRange = 1f;
    public LayerMask ItemLayer;

    private void Update()
    {
        CheckPickup();
    }
    private void CheckPickup()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, PickupRange, ItemLayer);

        foreach (Collider2D hit in hits)
        {
            Debug.Log(hit.gameObject.name);
            Gun gun = hit.GetComponent<Gun>();
            if (gun != null)
            {
                PlayerShoot playerShoot = GetComponent<PlayerShoot>();
                if (playerShoot != null)
                {
                    playerShoot.PickupGun(gun);
                    gun.gameObject.GetComponent<Collider2D>().enabled = false;
                    gun.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    }
}