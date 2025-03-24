using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    public LayerMask triggerMask;

    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object's layer is within the triggerMask
        if ((triggerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            OnEnter.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((triggerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            OnExit.Invoke();
        }
    }
}
