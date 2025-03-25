using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public string gunName;

    public float fireRate;

    public float accuracy;

    public float bulletPerShot;
    public float damage;
    public int clipSize;
    public int maxAmmo;
    public float reloadTime;

    public AmmoType ammoType;

    public float bulletSpeed;

    public bool isAutomatic;

    public AudioClip fireSound;

    public AudioClip ReloadSound;

}

public enum AmmoType
{
    SMG,
    Pistol,
    Shotgun
}
