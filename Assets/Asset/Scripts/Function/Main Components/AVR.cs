using UnityEngine;
using UnityEngine.InputSystem;

public class AVR : MonoBehaviour
{
    public SystemUnit systemUnit;
    public Motherboard motherboard;

    private InputAction mouseClickAction;

    private void Awake()
    {
        mouseClickAction = new InputAction("MouseClick", binding: "<Mouse>/leftButton");
        mouseClickAction.performed += ctx => HandleClick();
        mouseClickAction.Enable();
    }

    private void OnDestroy()
    {
        mouseClickAction.Disable();
    }

    private void HandleClick()
    {

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("AVR clicked!");

                // ✅ Check motherboard installation
                if (systemUnit == null || !systemUnit.IsMotherboardInstalled())
                {
                    TroubleshootManager.Instance.ShowMessage("Cannot turn on, motherboard not installed.", true);
                    return;
                }

                if (motherboard == null || motherboard.itemSlots == null || motherboard.itemSlots.Length == 0)
                {
                    Debug.LogError("Motherboard or slots not assigned!");
                    return;
                }

                // ✅ Validate each slot directly here
                foreach (ItemSlot slot in motherboard.itemSlots)
                {
                    if (slot.currentItem == null)
                    {
                        TroubleshootManager.Instance.ShowMessage($"Cannot turn on, {slot.name} is empty.", true);
                        return;
                    }

                    string actualName = slot.currentItem.name.Replace("(Clone)", "").Trim();
                    string expectedName = slot.expectedItemName.Trim();

                    if (!string.Equals(expectedName, actualName, System.StringComparison.OrdinalIgnoreCase))
                    {
                        TroubleshootManager.Instance.ShowMessage(
                            $"Cannot turn on, {slot.name} has wrong item (expected {expectedName}, got {actualName}).",
                            true
                        );
                        return;
                    }
                }

                // ✅ If we reach here, all slots are correct
                TroubleshootManager.Instance.ShowMessage("Hardware Assembly Complete.", false);
            }
        }
    }
}
