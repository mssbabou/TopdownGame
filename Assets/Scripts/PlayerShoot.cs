using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PlayerShoot : MonoBehaviour
{
    public List<Gun> guns = new List<Gun>();
    public List<MeleeWeapon> meleeWeapons = new List<MeleeWeapon>();
    private int currentGunIndex = -1;
    private int currentMeleeWeaponIndex = -1;
    public Slider reloadSlider;
    public RectTransform sliderTransform;

    void Start()
    {
        if (guns.Count > 0)
        {
            EquipGun(0);
        }
        if (meleeWeapons.Count > 0)
        {
            EquipMeleeWeapon(0);
        }
        if (reloadSlider != null)
        {
            reloadSlider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        HandleShooting();
        HandleReloading();
        HandleGunSwitching();
        HandleMeleeWeaponUsage();
        //HandleCrowbarEquip();
        UpdateReloadProgress();
        UpdateSliderPosition();
    }

    private void HandleShooting()
    {
        if (currentGunIndex >= 0 && currentGunIndex < guns.Count)
        {
            Gun currentGun = guns[currentGunIndex];
            if (currentGun.IsAutomatic() == true)
            {
                if (Input.GetMouseButton(0))
                {
                    currentGun.Fire();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentGun.Fire();
                }
            }
        }
    }

    public bool HasGunWithAmmoType(AmmoType ammoType)
    {
        return guns.Any(gun => gun != null && gun.gunData.ammoType == ammoType);
    }

    public void PickupAmmo(AmmoType ammoType, int amount)
    {
        foreach (Gun gun in guns)
        {
            if (gun != null && gun.gunData.ammoType == ammoType)
            {
                gun.AddAmmo(amount);
            }
        }
    }

    private void HandleMeleeWeaponUsage()
    {

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (currentMeleeWeaponIndex >= 0 && currentMeleeWeaponIndex < meleeWeapons.Count && meleeWeapons[currentMeleeWeaponIndex] != null)
            {
                meleeWeapons[currentMeleeWeaponIndex].Hit();
            }
        }

    }

    private void HandleReloading()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            guns[currentGunIndex].Reload();
        }
    }

    private void UpdateSliderPosition()
    {
        if (sliderTransform == null) return;

        // Update the slider position to be below the player
        Vector3 playerPosition = transform.position;
        sliderTransform.position = new Vector3(playerPosition.x, playerPosition.y - 1.0f, playerPosition.z);
        sliderTransform.rotation = Quaternion.identity;
    }

    private void HandleGunSwitching()
    {
        for (int i = 0; i < guns.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipGun(i);
                reloadSlider.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateReloadProgress()
    {
        if (currentGunIndex >= 0 && currentGunIndex < guns.Count && guns[currentGunIndex].IsReloading())
        {
            reloadSlider.gameObject.SetActive(true);

            float reloadProgress = guns[currentGunIndex].GetReloadProgress();
            reloadSlider.value = reloadProgress;

            if (reloadProgress >= 0.95f)
            {
                reloadSlider.gameObject.SetActive(false);
            }
        }
    }

    private void UnequipCurrentWeapon()
    {
        currentGunIndex = -1;
        currentMeleeWeaponIndex = -1;
    }

    void EquipGun(int index)
    {
        if (index >= 0 && index < guns.Count)
        {
            UnequipCurrentWeapon();
            currentGunIndex = index;

        }
        else
        {
            Debug.LogWarning("Attempted to equip a gun with an invalid index.");
        }
    }

    void EquipMeleeWeapon(int index)
    {
        if (index >= 0 && index < meleeWeapons.Count)
        {
            UnequipCurrentWeapon();
            currentMeleeWeaponIndex = index;

        }
        else
        {
            Debug.LogWarning("Attempted to equip a melee weapon with an invalid index.");
        }
    }

    public void PickupGun(Gun newGun)
    {
        guns.Add(newGun);
        EquipGun(guns.Count - 1);

    }

    public void PickupMeleeWeapon(MeleeWeapon newWeapon)
    {
        meleeWeapons.Add(newWeapon);
        EquipMeleeWeapon(meleeWeapons.Count - 1);


    }

    //private void HandleCrowbarEquip()
    //{
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        for (int i = 0; i < meleeWeapons.Count; i++)
    //       {
    //            if (meleeWeapons[i].weaponName == "Crowbar")
    //            {
    //                EquipMeleeWeapon(i);
    //
    //                break;
    //           }
    //       }
    //   }
    // }

    public int GetCurrentGunIndex()
    {
        return currentGunIndex;
    }

    public Gun GetCurrentGun()
    {
        if (currentGunIndex >= 0 && currentGunIndex < guns.Count)
        {
            return guns[currentGunIndex];
        }
        return null;
    }

}