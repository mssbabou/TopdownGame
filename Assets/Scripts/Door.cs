using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public Transform doorTransform;
    public Collider2D doorCollider;
    public Vector2 MoveOffset;
    public float Speed = 2f;


    [SerializeField] public bool IsOpen { get; private set; } = false;

    private Coroutine currentCoroutine;

    private Health health;

    void Start()
    {

        if (GetComponent<Health>() != null)
        {
            health = GetComponent<Health>();
            health.onDeath.AddListener(onDeath);
        }
    }
    public void Open()
    {
        if (IsOpen) return;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(MoveDoor(MoveOffset));
        IsOpen = true;

        doorCollider.enabled = false;
    }

    public void Close()
    {
        if (!IsOpen) return;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(MoveDoor(Vector3.zero));
        IsOpen = false;

        doorCollider.enabled = true;
    }

    public void Toggle()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
    void onDeath()
    {
        Destroy(gameObject);
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        Vector3 startPos = doorTransform.localPosition;
        float distance = Vector3.Distance(startPos, targetPosition);
        float elapsed = 0f;

        while (elapsed < distance / Speed)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed * Speed / distance);
            doorTransform.localPosition = Vector3.Lerp(startPos, targetPosition, t);
            yield return null;
        }

        doorTransform.localPosition = targetPosition;
        currentCoroutine = null;
    }
}