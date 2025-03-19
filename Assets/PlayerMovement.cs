using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MaxWalkSpeed = 110f;
    public float MaxRunSpeed = 110f;
    public float Acceleration = 250f;
    public float Deceleration = 200f;
    public float AngularSpeed = 10f;

    private Vector2 velocity;
    private float targetAngle;

    private Rigidbody2D rb;

    private float currentMaxSpeed;

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

        currentMaxSpeed = Input.GetKey(KeyCode.LeftShift) ? MaxRunSpeed : MaxWalkSpeed;

        // Normalize and store target angle
        if (isMoving)
        {
            targetVelocity = targetVelocity.normalized * currentMaxSpeed;
            targetAngle = Mathf.Atan2(targetVelocity.y, targetVelocity.x) * Mathf.Rad2Deg + 270;
        }

        // Accelerate or decelerate
        if (isMoving)
        {
            Vector2 accelDir = (targetVelocity - velocity).normalized;
            velocity += accelDir * Acceleration * Time.deltaTime;

            if (velocity.magnitude > currentMaxSpeed)
            {
                velocity = velocity.normalized * currentMaxSpeed;
            }
        }
        else
        {
            if (velocity.magnitude > 0)
            {
                Vector2 decelDir = -velocity.normalized;
                velocity += decelDir * Deceleration * Time.deltaTime;

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
                transform.Rotate(Vector3.forward, rotationDirection * AngularSpeed * Time.deltaTime);

                // Clamp rotation if overshot
                if (Mathf.Abs(angleDiff) < AngularSpeed * Time.deltaTime)
                {
                    transform.rotation = Quaternion.Euler(0, 0, targetAngle);
                }
            }
        }

        // Apply velocity
        rb.linearVelocity = velocity;
    }
}
