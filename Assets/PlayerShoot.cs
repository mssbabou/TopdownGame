using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

public class PlayerShoot : MonoBehaviour
{
    public List<Gun> guns = new List<Gun>();
    private int currentGunIndex;
    public Slider reloadSlider;
    public RectTransform sliderTransform;

    void Start()
    {
        if (guns.Count > 0)
        {
            EquipGun(0);
        }
        reloadSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleShooting();
        HandleReloading();
        HandleGunSwitching();
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
        else
        {
            Debug.LogWarning("No gun equipped.");
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

    void EquipGun(int index)
    {
        if (index >= 0 && index < guns.Count)
        {
            currentGunIndex = index;
            Debug.Log($"Equipped {guns[currentGunIndex].gunData.gunName}");
        }
        else
        {
            Debug.LogWarning("Attempted to equip a gun with an invalid index.");
        }
    }

    public void PickupGun(Gun newGun)
    {
        guns.Add(newGun);
        EquipGun(guns.Count - 1);
        Debug.Log($"Picked up and equipped {newGun.gunData.gunName}");
    }

    private void OnGUI()
    {
        if (guns.Count > 0 && currentGunIndex >= 0 && currentGunIndex < guns.Count)
        {
            GUIStyle style = new GUIStyle()
            {
                fontSize = 24,
                normal = new GUIStyleState() { textColor = Color.white }
            };

            GUI.Label(new Rect(10, Screen.height - 30, 200, 20), $"Ammo: {guns[currentGunIndex]?.GetCurrentAmmo()} / {guns[currentGunIndex]?.GetMaxAmmo()}", style);
        }
    }
}