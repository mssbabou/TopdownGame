using UnityEngine;
using UnityEngine.Events;

public class KeycardScanner : MonoBehaviour, Interactable
{
    public ItemType AcceptedKeycard;

    public bool Activated;
    public UnityEvent OnActivation;

    public void Interact(PlayerAction action)
    {
        if (Activated) return;

        if (action.Inventory.Contains(AcceptedKeycard)) 
        {
            Activated = true;
            OnActivation.Invoke();
        }
    }
}

interface Interactable
{
    void Interact(PlayerAction action);
}