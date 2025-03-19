using UnityEngine;

public class Bullet : MonoBehaviour
{
    float Damage;
    float Speed;


    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, RotationToDirection2D(transform.rotation.z), Speed * Time.deltaTime);
        if (hit)
        {
            Health health = hit.collider.gameObject.GetComponent<Health>();
            if (health)
            {
                health.TakeDamage(Damage);
            }
        }
        else
        {
            transform.position += Speed * Time.deltaTime * transform.up;  
        }
    }

    public void Init(float damage, float speed)
    {
        Damage = damage;
        Speed = speed;
    }

    Vector2 RotationToDirection2D(float angleDegrees)
    {
        // Convert the angle to radians and then get the unit vector
        float radians = angleDegrees * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        return direction;
    }
}
