using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 75f;
    public float acceleration = 250f;
    public float deceleration = 200f;
    public float angularSpeed = 10f;

    private Vector2 velocity;
    private float targetAngle;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetAngle = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        Vector2 targetVelocity = Vector2.zero;
        bool isMoving = false;

        if (Input.GetKey(KeyCode.D)) // Move Right
        {
            targetVelocity.x += 1;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.A)) // Move Left
        {
            targetVelocity.x -= 1;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.S)) // Move Down
        {
            targetVelocity.y -= 1;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.W)) // Move Up
        {
            targetVelocity.y += 1;
            isMoving = true;
        }

        // Normalize and store target angle
        if (isMoving)
        {
            targetVelocity = targetVelocity.normalized * maxSpeed;
            targetAngle = Mathf.Atan2(targetVelocity.y, targetVelocity.x) * Mathf.Rad2Deg + 270;
        }

        // Accelerate or decelerate
        if (isMoving)
        {
            Vector2 accelDir = (targetVelocity - velocity).normalized;
            velocity += accelDir * acceleration * Time.deltaTime;

            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }
        }
        else
        {
            if (velocity.magnitude > 0)
            {
                Vector2 decelDir = -velocity.normalized;
                velocity += decelDir * deceleration * Time.deltaTime;

                if (velocity.magnitude < 1)
                {
                    velocity = Vector2.zero;
                }
            }
        }

        // Rotate only if moving
        if (isMoving)
        {
            float angleDiff = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, targetAngle);

            if (Mathf.Abs(angleDiff) > 0.01f)
            {
                float rotationDirection = Mathf.Sign(angleDiff);
                transform.Rotate(Vector3.forward, rotationDirection * angularSpeed * Time.deltaTime);

                // Clamp rotation if overshot
                if (Mathf.Abs(angleDiff) < angularSpeed * Time.deltaTime)
                {
                    transform.rotation = Quaternion.Euler(0, 0, targetAngle);
                }
            }
        }

        // Apply velocity
        rb.linearVelocity = velocity;
    }
}
