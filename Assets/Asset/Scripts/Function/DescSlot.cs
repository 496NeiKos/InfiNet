using UnityEngine;
using UnityEngine.UI;

public class DescSlot : MonoBehaviour
{
    public DragItem currentItem = null;
    [Header("UI Output")]
    public Text terminalText;

    public void SetCurrentItem(DragItem item)
    {
        // ✅ If slot already has an item, eject it first
        if (currentItem != null)
        {
            Debug.Log($"[DescSlot] {name}: Already has {currentItem.name}, ejecting before setting new item.");
            ClearSlot();
        }

        // ✅ Snap new item into slot
        currentItem = item;
        currentItem.transform.position = transform.position;
        currentItem.transform.SetParent(transform);

        Debug.Log($"[DescSlot] {name}: Set current item to {item.name}. Parent is now {currentItem.transform.parent?.name}");

        // ✅ Show description in terminal
        ComponentItem compItem = item.GetComponent<ComponentItem>();
        if (compItem != null && terminalText != null)
        {
            terminalText.color = Color.white;
            terminalText.text = compItem.GetDescription();
            Debug.Log($"[DescSlot] {name}: Showing description for {item.name} → {compItem.GetDescription()}");
        }
    }

    public void ClearSlot()
    {
        if (currentItem != null)
        {
            Debug.Log($"[DescSlot] {name}: Clearing {currentItem.name}. Parent before eject: {currentItem.transform.parent?.name}");

            // ✅ Always return to ItemArea parent
            if (currentItem.itemArea != null)
            {
                currentItem.transform.SetParent(currentItem.itemArea.transform);
                currentItem.transform.position = currentItem.itemArea.transform.position;
                Debug.Log($"[DescSlot] {name}: {currentItem.name} re‑parented to ItemArea {currentItem.itemArea.name}. Parent after eject: {currentItem.transform.parent?.name}");
            }
            else
            {
                currentItem.transform.SetParent(null);
                Debug.Log($"[DescSlot] {name}: {currentItem.name} detached completely. Parent after eject: {currentItem.transform.parent?.name}");
            }

            // ✅ Clear reference
            currentItem = null;
            //Debug.Log($"[DescSlot] {name}: currentItem reference cleared.");

            //// ✅ Clear terminal text
            //if (terminalText != null)
            //{
            //    terminalText.text = "";
            //    Debug.Log($"[DescSlot] {name}: Terminal text cleared.");
            //}
        }
        else
        {
            Debug.Log($"[DescSlot] {name}: ClearSlot called but no currentItem assigned.");
        }
    }

    private void Update()
    {
        // ✅ Auto‑clear if item was dragged out manually
        if (currentItem != null && currentItem.transform.parent != transform)
        {
            Debug.Log($"[DescSlot] {name}: Detected {currentItem.name} is no longer a child. Auto‑clearing reference.");
            currentItem = null;

            //if (terminalText != null)
            //{
            //    terminalText.text = "";
            //    Debug.Log($"[DescSlot] {name}: Terminal text cleared via auto‑clear.");
            //}
        }
    }
}
