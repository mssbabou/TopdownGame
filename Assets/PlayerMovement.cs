using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MaxWalkSpeed = 7.5f;
    public float MaxRunSpeed = 11f;
    public float Acceleration = 10f;
    public float Deceleration = 10f;
    public float RotationSpeed = 5f; // Smooth rotation speed

    private Vector2 movement;
    private Vector2 velocity;
    private float targetAngle;
    private bool isAiming;

    private Rigidbody2D rb;

    private TrainMovement trainMovement;
    private Collider2D trainCollider;
    private bool isPlayerOnTrain;

    private float currentMaxSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trainMovement = FindFirstObjectByType<TrainMovement>();
        if (trainMovement != null) trainCollider = trainMovement.GetComponent<Collider2D>();
    }

    void UpdateTrainPhysics()
    {
        if (trainMovement == null || trainCollider == null)
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

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        // Check for aiming input
        isAiming = Input.GetMouseButton(1);

        // Set movement speed based on run or walk
        currentMaxSpeed = Input.GetKey(KeyCode.LeftShift) ? MaxRunSpeed : MaxWalkSpeed;
    }

    void FixedUpdate()
    {
        bool isMoving = movement.magnitude > 0.1f;

        Vector2 targetVelocity = Vector2.zero;

        // Normalize and store the target velocity and angle when moving
        if (isMoving)
        {
            targetVelocity = movement * currentMaxSpeed;
        }

        // Smooth acceleration
        if (isMoving)
        {
            velocity = Vector2.MoveTowards(velocity, targetVelocity, Acceleration * Time.fixedDeltaTime);
        }
        else
        {
            velocity = Vector2.MoveTowards(velocity, Vector2.zero, Deceleration * Time.fixedDeltaTime);
        }

        // Handle rotation
        if (isAiming)
        {
            // Rotate towards mouse position when aiming
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = mousePosition - (Vector2)transform.position;
            targetAngle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg + 270;
        }
        else if (isMoving)
        {
            // Rotate towards movement direction when not aiming
            targetAngle = Mathf.Atan2(targetVelocity.y, targetVelocity.x) * Mathf.Rad2Deg + 270;
        }

        // Smooth rotation
        if (Mathf.Abs(Mathf.DeltaAngle(rb.rotation, targetAngle)) > 0.1f)
        {
            if (isMoving || isAiming) 
                rb.rotation = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, RotationSpeed * Time.fixedDeltaTime);
        }

        // Apply movement based on whether the player is on the train or not
        if (isPlayerOnTrain)
        {
            Vector2 newVelocity = velocity + trainMovement.CurrentVelocity;
            rb.MovePosition(transform.position + (Vector3)newVelocity * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(transform.position + (Vector3)velocity * Time.fixedDeltaTime);
        }
    }
}
