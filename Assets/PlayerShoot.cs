using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Gun[] guns;
    private int currentGunIndex;

    void Start()
    {
        if (guns.Length > 0)
        {
            EquipGun(0);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            guns[currentGunIndex].Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            guns[currentGunIndex].Reload();
        }

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

    void EquipGun(int index)
    {
        if (index >= 0 && index < guns.Length)
        {
            currentGunIndex = index;
            // Optionally, you can add logic to visually equip the gun, e.g., enabling/disabling gun GameObjects
            Debug.Log($"Equipped {guns[currentGunIndex].gunData.gunName}");
        }
    }
}