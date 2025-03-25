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
    private AudioSource audioSource;

    public AudioClip EmptyMag;

    public bool IsAutomatic()
    {
        return gunData.isAutomatic;
    }

    private void Start()
    {
        currentAmmo = gunData.clipSize;
        maxAmmo = gunData.maxAmmo;
        audioSource = gameObject.AddComponent<AudioSource>();
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
            if (gunData.fireSound != null)
            {
                audioSource.PlayOneShot(gunData.fireSound);
            }
        }
        else
        {
            Reload();
            if (EmptyMag != null)
            {
                audioSource.PlayOneShot(EmptyMag);
            }
        }
    }

    public void Reload()
    {
        if (currentAmmo >= gunData.clipSize)
        {

            return;
        }

        if (maxAmmo == 0)
        {

            return;
        }

        if (!isReloading)
        {
            isReloading = true;
            if (gunData.ReloadSound != null)
            {
                audioSource.PlayOneShot(gunData.ReloadSound);
            }
            reloadEndTime = Time.time + gunData.reloadTime;
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

        }
        else
        {
            currentAmmo += maxAmmo;

            maxAmmo = 0;
        }
    }
    public void AddAmmo(int amount)
    {
        maxAmmo += amount;
        Debug.Log($"Added {amount} ammo to {gunData.gunName}. Total ammo: {maxAmmo}");
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