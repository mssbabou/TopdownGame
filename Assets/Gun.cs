using UnityEngine;

public class Gun : MonoBehaviour, IGun
{
    public GunData gunData;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    private int currentAmmo;
    private float nextFireTime;
    private float reloadEndTime;
    private bool isReloading = false;

    private int maxAmmo;

    private void Start()
    {
        currentAmmo = gunData.clipSize;
        maxAmmo = gunData.maxAmmo;
    }

    private void Update()
    {
        if (isReloading && Time.time >= reloadEndTime)
        {
            isReloading = false;
            Debug.Log($"Finished reloading {gunData.gunName}");
        }
    }

    public void Fire()
    {
        if (isReloading)
        {
            return;
        }

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
            Reload();
        }
    }

    public void Reload()
    {
        if (maxAmmo == 0)
    {
        Debug.Log("No ammo left to reload.");
        return;
    }
        if (!isReloading)
        {
            isReloading = true;
            reloadEndTime = Time.time + gunData.reloadTime;
            Debug.Log($"Reloading {gunData.gunName}");
            if (currentAmmo < gunData.clipSize) 
        {
            int ammoNeeded = gunData.clipSize - currentAmmo;

            if (maxAmmo >= ammoNeeded) 
            {
                currentAmmo = gunData.clipSize;  
                maxAmmo -= ammoNeeded;  
                print($"Reloaded. Current ammo in clip: {currentAmmo}, Total ammo left: {maxAmmo}");
            }
            else
            {
                currentAmmo += maxAmmo;  
                print($"Partially reloaded. Current ammo in clip: {currentAmmo}, Total ammo left: 0");
                maxAmmo = 0;
            }
        }
        else
        {
            print($"Clip is already full. Current ammo in clip: {currentAmmo}");
        }
    }
}
public float GetReloadProgress()
{
    if (!isReloading)
    {
        return 0f;
    }
    return 1f - (reloadEndTime - Time.time) / gunData.reloadTime;
}
        
 
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }}
