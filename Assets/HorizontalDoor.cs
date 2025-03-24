using UnityEngine;
using System.Collections;

public class HorizontalDoor : MonoBehaviour
{
    public Transform doorTransform;
    public float MoveOffset = 3f;
    public float Speed = 2f;

    [SerializeField] public bool IsOpen { get; private set; } = false;

    private Coroutine currentCoroutine;

    public void Open()
    {
        if (IsOpen) return;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(MoveDoor(Vector3.up * MoveOffset));
        IsOpen = true;
    }

    public void Close()
    {
        if (!IsOpen) return;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(MoveDoor(Vector3.zero));
        IsOpen = false;
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

