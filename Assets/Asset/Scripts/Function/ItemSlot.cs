using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public DragItem currentItem = null;

    // ✅ Expected item for this slot (e.g., "CPU", "Heatsink")
    public string expectedItemName;

    // ✅ Optional dependency: e.g., heatsink requires CPU slot
    public ItemSlot requiredItemSlot;
    public string requiredItemName;

    public void SetCurrentItem(DragItem item)
    {
        // ✅ Check dependency before allowing install
        if (requiredItemSlot != null)
        {
            if (requiredItemSlot.currentItem == null ||
                !string.Equals(requiredItemSlot.currentItem.name.Replace("(Clone)", ""), requiredItemName, System.StringComparison.OrdinalIgnoreCase))
            {
                TroubleshootManager.Instance.ShowMessage(
                    $"Cannot install {item.name} because {requiredItemName} must be installed in {requiredItemSlot.name} first.",
                    true
                );

                // Snap back to item area instead of staying in slot
                item.transform.SetParent(item.itemArea.transform);
                item.transform.position = item.itemArea.transform.position;

                // ✅ Revert sprite since install failed
                item.SetToDefaultSprite();
                return;
            }
        }

        currentItem = item;
        Debug.Log($"ItemSlot {name}: Set current item to {item.name}");

        // ✅ Change sprite when item is successfully installed
        currentItem.SetToSlotSprite();
    }

    public void ClearSlot()
    {
        // ✅ Block removal if a dependent item is still installed
        if (HasDependentItem())
        {
            TroubleshootManager.Instance.ShowMessage(
                $"Cannot remove {currentItem?.name} because {name} has a dependent item installed (e.g., heatsink).",
                true
            );

            if (currentItem != null)
            {
                currentItem.transform.position = transform.position;
                currentItem.transform.SetParent(transform);
            }
            return;
        }

        // ✅ Block motherboard removal if AVR is ON
        AVR avr = FindObjectOfType<AVR>();
        if (avr != null && avr.IsOn() &&
            string.Equals(expectedItemName.Trim(), "Motherboard", System.StringComparison.OrdinalIgnoreCase))
        {
            TroubleshootManager.Instance.ShowMessage(
                "Cannot remove Motherboard while AVR is turned ON. Please turn off AVR first.",
                true
            );

            if (currentItem != null)
            {
                currentItem.transform.position = transform.position;
                currentItem.transform.SetParent(transform);
            }
            return;
        }

        Debug.Log($"ItemSlot {name}: Cleared slot");

        if (currentItem != null)
        {
            currentItem.SetToDefaultSprite();
        }

        currentItem = null;
    }




    private bool HasDependentItem()
    {
        // ✅ Check if another slot depends on this one and is occupied
        ItemSlot[] allSlots = FindObjectsOfType<ItemSlot>();
        foreach (ItemSlot slot in allSlots)
        {
            if (slot.requiredItemSlot == this && slot.currentItem != null)
            {
                return true;
            }
        }
        return false;
    }
}
