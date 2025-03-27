using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    public PlayerShoot PS;

    public GameObject AmmoPanel;
    public TextMeshProUGUI AmmoText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PS = FindFirstObjectByType<PlayerShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PS == null) return;

        Gun gun = PS.GetCurrentGun();
        if (gun == null)
        {
            AmmoPanel.SetActive(false);
            return;
        }

        AmmoPanel.SetActive(true);

        AmmoText.text = $"{gun.GetCurrentAmmo()} - {gun.GetMaxAmmo()}";
    }
}
