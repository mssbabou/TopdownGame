using TMPro;
using UnityEngine;

public class UI_Ammo : MonoBehaviour
{
    public PlayerShoot PA;
    public TextMeshProUGUI ammoText;

    void Start()
    {
        PA = FindAnyObjectByType<PlayerShoot>();
        if (PA == null)
        {
            Debug.LogError("PlayerShoot component not found in the scene.");
        }
    }

    void Update()
    {
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        if (PA == null || ammoText == null) return;

        Gun currentGun = PA.GetCurrentGun();
        if (currentGun != null)
        {
            ammoText.text = $"{currentGun.GetCurrentAmmo()} / {currentGun.GetMaxAmmo()}";
        }
        else
        {
            ammoText.text = "No Gun Equipped";
        }
    }
}