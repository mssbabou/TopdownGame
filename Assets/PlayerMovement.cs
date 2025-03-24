using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MaxWalkSpeed = 7.5f;
    public float MaxRunSpeed = 11f;
    public float Acceleration = 250f;
    public float Deceleration = 200f;
    public float AngularSpeed = 500f; // Angular speed for smooth rotation

    private Vector2 velocity;
    private float targetAngle;

    private Rigidbody2D rb;

    private TrainMovement trainMovement;
    private Collider2D trainCollider;
    private bool isPlayerOnTrain;

    private float currentMaxSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetAngle = transform.rotation.eulerAngles.z;

        trainMovement = FindFirstObjectByType<TrainMovement>();
        if (trainMovement != null) trainCollider = trainMovement.GetComponent<Collider2D>();
    }

    void UpdateTrainPhysics()
    {
        if (trainMovement == null ||  trainCollider == null)
        {
            isPlayerOnTrain = false;
            return;
        }

        isPlayerOnTrain = trainCollider.OverlapPoint(transform.position);
        trainMovement.SetPlayerOnTrain(isPlayerOnTrain);
    }

    void Update()
    {
        UpdateTrainPhysics();

        Vector2 targetVelocity = Vector2.zero;
        bool isMoving = false;
        bool isAiming = false;

        // Handle movement input
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

        // Check for aiming input
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
        }

        // Set movement speed based on run or walk
        currentMaxSpeed = Input.GetKey(KeyCode.LeftShift) ? MaxRunSpeed : MaxWalkSpeed;

        // Normalize and store the target velocity and angle when moving
        if (isMoving)
        {
            targetVelocity = targetVelocity.normalized * currentMaxSpeed;
        }

        // Accelerate or decelerate based on movement
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

        // Handle rotation
        if (isAiming)
        {
            // Rotate towards mouse position when aiming
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = mousePosition - (Vector2)transform.position;
            targetAngle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg + 270;

            RotateTowardsTargetAngle();
        }
        else if (isMoving)
        {
            // Rotate towards movement direction when not aiming
            targetAngle = Mathf.Atan2(targetVelocity.y, targetVelocity.x) * Mathf.Rad2Deg + 270;
            RotateTowardsTargetAngle();
        }

        if (isPlayerOnTrain)
        {
            rb.linearVelocity = velocity + trainMovement.CurrentVelocity;
        }
        else 
        {
            rb.linearVelocity = velocity;
        }
    }

    // Apply smooth rotation towards target angle at fixed angular speed
    void RotateTowardsTargetAngle()
    {
        float currentAngle = transform.rotation.eulerAngles.z;
        float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);

        if (Mathf.Abs(angleDiff) > 0.01f)
        {
            float rotationDirection = Mathf.Sign(angleDiff);
            float angularVelocity = rotationDirection * AngularSpeed * Time.deltaTime;

            // Use Rigidbody2D to rotate smoothly
            rb.MoveRotation(rb.rotation + angularVelocity);

            // Clamp rotation if overshot
            if (Mathf.Abs(angleDiff) < AngularSpeed * Time.deltaTime)
            {
                rb.rotation = targetAngle;
            }
        }
    }
}
