using UnityEngine;
using UnityEngine.UI; // Add this to use the Slider class

public class PlayerShoot : MonoBehaviour
{
    public Gun[] guns;
    private int currentGunIndex;
    public Slider reloadSlider; // Add this line
    private bool isReloading = false; // Add this line
    public RectTransform sliderTransform;

    void Start()
    {
        if (guns.Length > 0)
        {
            EquipGun(0);
        }
        reloadSlider.gameObject.SetActive(false); // Hide the slider initially
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
        if (Input.GetMouseButtonDown(0))
        {
            guns[currentGunIndex].Fire();
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
        // Update the slider position to be below the player
         Vector3 playerPosition = transform.position;
        sliderTransform.position = new Vector3(playerPosition.x, playerPosition.y - 1.0f, playerPosition.z);
        sliderTransform.rotation = Quaternion.identity;
    }

    private void HandleGunSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipGun(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && guns.Length > 1)
        {
            EquipGun(1);
        }
        // Add more keys for additional guns if needed
    }

    private void UpdateReloadProgress()
    {
        if (guns[currentGunIndex].IsReloading())
        {
            reloadSlider.gameObject.SetActive(true);

            float reloadProgress = guns[currentGunIndex].GetReloadProgress();
            reloadSlider.value = reloadProgress;

            if (reloadProgress >= 0.95f)
            {
                isReloading = false; // Reset the flag when reloading is complete
                reloadSlider.gameObject.SetActive(false); // Hide the slider
            }
        }
    }

    void EquipGun(int index)
    {
        if (index >= 0 && index < guns.Length)
        {
            currentGunIndex = index;
            // Optionally, you can add logic to visually equip the gun, e.g., enabling/disabling gun GameObjects
            Debug.Log($"Equipped {guns[currentGunIndex].gunData.gunName}");
        }
    }
    public void PickupGun(Gun newGun)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i] == null)
            {
                guns[i] = newGun;
                EquipGun(i);
                Debug.Log($"Picked up and equipped {newGun.gunData.gunName}");
                return;
            }
        }
        Debug.Log("No available slots to pick up the gun.");
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(10, Screen.height - 30, 200, 20), $"Ammo: {guns[currentGunIndex].GetCurrentAmmo()} / {guns[currentGunIndex].GetMaxAmmo()}", style);
    }
}