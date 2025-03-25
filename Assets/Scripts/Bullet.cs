using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject ImpactEffect;

    float Damage;
    float Speed;
    float MaxDistance = 20f;

    float distanceTraveled;

    LayerMask layermask;

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, RotationToDirection2D(transform.rotation.z), Speed * Time.deltaTime, ~layermask);
        if (hit)
        {
            Health health = hit.collider.gameObject.GetComponent<Health>();
            if (health)
            {
                health.TakeDamage(Damage);
            }

            Impact();
        }
        else
        {
            transform.position += Speed * Time.deltaTime * transform.up;
            distanceTraveled += Speed * Time.deltaTime;

            if (distanceTraveled >= MaxDistance) DeSpawn();
        }
    }

    public void Init(float damage, float speed, LayerMask layerMask)
    {
        Damage = damage;
        Speed = speed;
        this.layermask = layerMask;
    }

    private void Impact()
    {
        // Spawn Impact Effect
        if (ImpactEffect != null) Instantiate(ImpactEffect, transform.position, transform.rotation);

        DeSpawn();
    }

    private void DeSpawn()
    {
        Destroy(gameObject);
    }

    Vector2 RotationToDirection2D(float angleDegrees)
    {
        // Convert the angle to radians and then get the unit vector
        float radians = angleDegrees * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        return direction;
    }
}