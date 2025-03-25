using UnityEngine;

public class Gun : MonoBehaviour, IGun
{
    public GunData gunData;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading = false;
    private int maxAmmo;
    private float reloadEndTime;

    public bool IsAutomatic()
    {
        return gunData.isAutomatic;
    }

    private void Start()
    {
        currentAmmo = gunData.clipSize;
        maxAmmo = gunData.maxAmmo;
    }

    private void Update()
    {
        if (isReloading && Time.time >= reloadEndTime)
        {
            FinishReloading();
        }

    }

    public void Fire()
    {
        if (isReloading)
        {
            Debug.Log("Cannot fire while reloading.");
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
        if (currentAmmo >= gunData.clipSize)
        {
            Debug.Log("Clip is already full.");
            return;
        }

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
        }
    }

    private void FinishReloading()
    {
        isReloading = false;
        int ammoNeeded = gunData.clipSize - currentAmmo;

        if (maxAmmo >= ammoNeeded)
        {
            currentAmmo = gunData.clipSize;
            maxAmmo -= ammoNeeded;
            Debug.Log($"Reloaded. Current ammo in clip: {currentAmmo}, Total ammo left: {maxAmmo}");
        }
        else
        {
            currentAmmo += maxAmmo;
            Debug.Log($"Partially reloaded. Current ammo in clip: {currentAmmo}, Total ammo left: 0");
            maxAmmo = 0;
        }

        Debug.Log($"Finished reloading {gunData.gunName}");
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

    public bool CanReload()
    {
        return currentAmmo < maxAmmo && !isReloading;
    }

    public bool IsReloading()
    {
        return isReloading;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }
}