using UnityEngine;
using UnityEngine.Events;

public class ToggleButton : MonoBehaviour, Interactable
{
    public UnityEvent<bool> OnChanged;
    public bool Activated;

    public void Interact(PlayerAction action)
    {
        Activated = !Activated;
        OnChanged.Invoke(Activated);
    }
}
