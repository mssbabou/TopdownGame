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

    private void OnGUI()
{
    GUIStyle style = new GUIStyle();
    style.fontSize = 24;
    style.normal.textColor = Color.white;

    GUI.Label(new Rect(10, Screen.height - 30, 200, 20), $"Ammo: {guns[currentGunIndex].GetCurrentAmmo()} / {guns[currentGunIndex].GetMaxAmmo()}", style);

    float reloadProgress = guns[currentGunIndex].GetReloadProgress();
    if (reloadProgress > 0)
    {
        GUI.Box(new Rect(10, Screen.height - 60, 200, 20), "");
        GUI.Box(new Rect(10, Screen.height - 60, 200 * reloadProgress, 20), "", style);
    }
}
}