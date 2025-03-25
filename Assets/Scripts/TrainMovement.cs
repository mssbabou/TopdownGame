using UnityEngine;
using System.Collections;

public class TrainMovement : MonoBehaviour
{
    public Transform[] TrainStops;
    public int WaitForPlayerToLeaveAtIndex = 1;
    public float TrainStopWaitTime = 3f;

    public float MaxSpeed = 5f;  // Maximum speed the train can reach
    public float Acceleration = 1f;  // Acceleration rate
    public float Deceleration = 2f;  // Deceleration rate
    public bool StartAtMaxSpeed = false;  // Flag to start at max speed

    private float currentSpeed = 0f;  // Current speed of the train
    private int currentStopIndex = 0;
    private bool isMoving = false;
    public bool IsPlayerOnTrain = false;

    private Collider2D trainCollider;

    // New variable to track the movement direction
    private Vector2 currentDirection = Vector2.zero;
    // Public property exposing the current velocity as a Vector2 (direction * speed)
    public Vector2 CurrentVelocity
    {
        get { return currentDirection * currentSpeed; }
    }

    void Awake()
    {
        if (StartAtMaxSpeed)
        {
            currentSpeed = MaxSpeed;
        }
        trainCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (TrainStops.Length == 0) return;  // No stops to move towards

        // Ensure the train starts by moving towards the first stop, even if it's not at TrainStops[0]
        if (!isMoving && currentStopIndex == 0)
        {
            StartCoroutine(MoveToStop(TrainStops[0].position));  // Move to the first stop
        }

        // Once at the first stop, proceed with the rest of the journey
        if (!isMoving && currentStopIndex < TrainStops.Length)
        {
            if (currentStopIndex == WaitForPlayerToLeaveAtIndex && IsPlayerOnTrain)
            {
                // Wait until the player leaves before starting to move
                return;
            }

            StartCoroutine(MoveToStop(TrainStops[currentStopIndex].position));
        }
    }

    // Coroutine to move the train towards a specific stop
    private IEnumerator MoveToStop(Vector2 targetPosition)
    {
        isMoving = true;

        float distance = Vector2.Distance(transform.position, targetPosition);

        while (distance > 0.1f)  // Move towards the target until we're close enough
        {
            // Update the movement direction each frame
            currentDirection = (targetPosition - (Vector2)transform.position).normalized;

            // Determine whether to decelerate or accelerate
            if (distance < currentSpeed * currentSpeed / (2 * Deceleration))
            {
                currentSpeed -= Deceleration * Time.deltaTime;
            }
            else
            {
                currentSpeed = Mathf.Min(currentSpeed + Acceleration * Time.deltaTime, MaxSpeed);
            }

            float step = currentSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);

            distance = Vector2.Distance(transform.position, targetPosition);

            yield return null;  // Wait for the next frame
        }

        // Reset direction when stopping
        currentDirection = Vector2.zero;

        // Wait for a while at the stop
        yield return new WaitForSeconds(TrainStopWaitTime);

        // If we need to wait for the player to leave at the current stop
        if (currentStopIndex == WaitForPlayerToLeaveAtIndex)
        {
            yield return new WaitUntil(() => !IsPlayerOnTrain);
        }

        // Move to the next stop or stop if at the end of the array
        currentStopIndex++;
        if (currentStopIndex < TrainStops.Length)
        {
            StartCoroutine(MoveToStop(TrainStops[currentStopIndex].position));
        }
        else
        {
            isMoving = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (TrainStops.Length == 0) return;

        for (int i = 0; i < TrainStops.Length - 1; i++)
        {
            Gizmos.color = i % 2 == 0 ? Color.red : Color.green;
            Gizmos.DrawLine(TrainStops[i].position, TrainStops[i + 1].position);
            Vector3 direction = (TrainStops[i + 1].position - TrainStops[i].position).normalized;
            Gizmos.DrawRay(TrainStops[i].position + direction * 0.5f, direction * 0.5f);
        }

        foreach (var stop in TrainStops)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(stop.position, 0.3f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, TrainStops[0].position);
    }

    // A method to update whether the player is on the train
    public void SetPlayerOnTrain(bool isOnTrain)
    {
        IsPlayerOnTrain = isOnTrain;
    }
}
