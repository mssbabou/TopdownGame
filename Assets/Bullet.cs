using UnityEngine;

public class Bullet : MonoBehaviour
{
    float Damage;
    float Speed;

    void Update()
    {
        transform.position += Speed * Time.deltaTime * transform.up;  
    }

    public void Init(float damage, float speed)
    {
        Damage = damage;
        Speed = speed;
    }
}
