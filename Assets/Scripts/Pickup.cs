using UnityEngine;

public class PickupGun : MonoBehaviour
{
    public float PickupRange = 1f;
    public LayerMask ItemLayer;

    private void Update()
    {
        CheckGunPickup();
    }

    private void CheckGunPickup()
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
                continue;
            }


            if (hit.CompareTag("SHOTGUN Ammo") || hit.CompareTag("SMG Ammo") || hit.CompareTag("Pistol Ammo"))
            {
                PlayerShoot playerShoot = GetComponent<PlayerShoot>();
                if (playerShoot != null)
                {
                    AmmoType ammoType;
                    int ammoAmount;

                    if (hit.CompareTag("SHOTGUN Ammo"))
                    {
                        ammoType = AmmoType.Shotgun;
                        ammoAmount = 10;
                    }
                    else if (hit.CompareTag("SMG Ammo"))
                    {
                        ammoType = AmmoType.SMG;
                        ammoAmount = 30;
                    }
                    else if (hit.CompareTag("Pistol Ammo"))
                    {
                        ammoType = AmmoType.Pistol;
                        ammoAmount = 15;
                    }
                    else
                    {
                        continue;
                    }

                    if (playerShoot.HasGunWithAmmoType(ammoType))
                    {
                        playerShoot.PickupAmmo(ammoType, ammoAmount);
                        hit.gameObject.GetComponent<Collider2D>().enabled = false;
                        hit.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }
        }
    }
}