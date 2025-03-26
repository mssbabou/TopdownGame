using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public PlayerAction PA;

    public GameObject RedKeycardUI;
    public GameObject GreenKeycardUI;
    public GameObject BlueKeycardUI;

    private void Start()
    {
        PA = FindAnyObjectByType<PlayerAction>();
    }

    private void Update()
    {
        if (PA == null) return;

        RedKeycardUI.SetActive(PA.Inventory.Contains(ItemType.RedKeycard));
        GreenKeycardUI.SetActive(PA.Inventory.Contains(ItemType.GreenKeycard));
        BlueKeycardUI.SetActive(PA.Inventory.Contains(ItemType.BlueKeycard));
    }
}
