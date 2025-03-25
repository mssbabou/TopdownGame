using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject StaminaSliderGameobject;

    public float MaxWalkSpeed = 7.5f;
    public float MaxRunSpeed = 11f;
    public float Acceleration = 10f;
    public float Deceleration = 10f;
    public float RotationSpeed = 5f; // Smooth rotation speed

    public float MaxStamina = 5f; // In Seconds
    public float StaminaRegen = 1f; // In Seconds gained per Second

    private Vector2 movement;
    private Vector2 velocity;
    private float targetAngle;
    private bool isAiming;

    private Rigidbody2D rb;

    private Slider staminaSlider;

    private TrainMovement trainMovement;
    private Collider2D trainCollider;
    private bool isPlayerOnTrain;

    private float currentMaxSpeed;
    private float currentStamina;
    private bool isExhausted;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trainMovement = FindFirstObjectByType<TrainMovement>();

        if (trainMovement != null) trainCollider = trainMovement.GetComponent<Collider2D>();
        if (StaminaSliderGameobject != null) 
        {
            staminaSlider = StaminaSliderGameobject.GetComponent<Slider>();
            StaminaSliderGameobject.SetActive(false);
        }

        currentStamina = MaxStamina;
    }

    void UpdateTrainPhysics()
    {
        if (trainMovement == null || trainCollider == null)
        {
            isPlayerOnTrain = false;
            return;
        }

        isPlayerOnTrain = trainCollider.OverlapPoint(rb.position);
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
        if (isExhausted)
        {
            // Force walking and regenerate stamina.
            currentMaxSpeed = MaxWalkSpeed;
            currentStamina += Time.deltaTime;

            // Only allow running again when stamina is completely full.
            if (currentStamina >= MaxStamina)
            {
                currentStamina = MaxStamina;  // Ensure it doesn't exceed the max.
                isExhausted = false;
            }
        }
        else
        {
            // If Left Shift is held and there's enough stamina, run.
            if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0.1f)
            {
                currentMaxSpeed = MaxRunSpeed;
                currentStamina -= Time.deltaTime;

                // If stamina depletes to the threshold, mark as exhausted.
                if (currentStamina <= 0.1f)
                {
                    isExhausted = true;
                }
            }
            else
            {
                // Otherwise, walk and regenerate stamina.
                currentMaxSpeed = MaxWalkSpeed;
                currentStamina += Time.deltaTime;
            }
        }

        // Clamp the stamina to ensure it remains between 0 and MaxStamina.
        currentStamina = Mathf.Clamp(currentStamina, 0, MaxStamina);

        if (StaminaSliderGameobject != null && staminaSlider != null)
        {
            if (currentStamina >= MaxStamina)
            {
                StaminaSliderGameobject.SetActive(false);
            }
            else
            {
                StaminaSliderGameobject.SetActive(true);
                StaminaSliderGameobject.transform.position = new Vector3(transform.position.x, transform.position.y - 1.3f, transform.position.z);
                StaminaSliderGameobject.transform.rotation = Quaternion.identity;
                staminaSlider.value = currentStamina / MaxStamina;
            }
        }
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
            Vector2 directionToMouse = mousePosition - (Vector2)rb.position;
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
            rb.MovePosition(rb.position + newVelocity * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }
}
