using UnityEngine;

public class Gun : MonoBehaviour, IGun
{
    public GunData gunData;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    private int currentAmmo;
    private float nextFireTime;

    private void Start()
    {
        currentAmmo = gunData.clipSize;
    }

    public void Fire()
    {
        if (Time.time < nextFireTime)
        {
            return;
        }

        if (currentAmmo > 0)
        {
            currentAmmo--;
            nextFireTime = Time.time + 1f / gunData.fireRate;

            for (int i = 0; i < gunData.bulletPerShot; i++)
            {

                var bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
                var bulletScript = bullet.GetComponent<Bullet>();
                bullet.transform.Rotate(0, 0, Random.Range(-gunData.accuracy / 2, gunData.accuracy / 2));
                bulletScript.Init(gunData.damage, gunData.bulletSpeed, LayerMask.GetMask("Player"));


            }
        }
        else
        {
            Debug.Log("Out of ammo, reload!");
        }
    }

    public void Reload()
    {

        currentAmmo = gunData.clipSize;
        Debug.Log($"Reloading {gunData.gunName}");
    }
}