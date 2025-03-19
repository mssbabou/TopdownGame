using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform ShootPoint;
    public GameObject BulletPrefab;

    public float BulletDamage;
    public float BulletSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var bullet = Instantiate(BulletPrefab, ShootPoint.position, ShootPoint.rotation);
            var bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Init(BulletDamage, BulletSpeed);
        }
    }
}
