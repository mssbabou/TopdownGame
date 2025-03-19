using UnityEngine;

public class Bullet : MonoBehaviour
{
    float Damage;
    float Speed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.up * Speed * Time.deltaTime;  
    }

    public void Init(float damage, float speed)
    {
        Damage = damage;
        Speed = speed;
    }
}
