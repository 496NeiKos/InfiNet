using UnityEngine;
using UnityEngine.InputSystem;

public class AVR : MonoBehaviour
{
    public SystemUnit systemUnit;
    public Motherboard motherboard;
    public VGACable vgaCable;

    [Header("AVR Sprites")]
    public Sprite lightsOffSprite;
    public Sprite lightsOnSprite;
    private SpriteRenderer spriteRenderer;

    [Header("Monitor Sprites")]
    public Sprite monitorOffSprite;
    public Sprite monitorOnSprite;
    public SpriteRenderer monitorRenderer;   // assign in Inspector

    private InputAction mouseClickAction;
    private bool isOn = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (lightsOffSprite == null && spriteRenderer != null)
        {
            lightsOffSprite = spriteRenderer.sprite; // fallback
        }

        mouseClickAction = new InputAction("MouseClick", binding: "<Mouse>/leftButton");
        mouseClickAction.performed += ctx => HandleClick();
        mouseClickAction.Enable();
    }

    private void OnDestroy()
    {
        mouseClickAction.Disable();
    }

    public bool IsOn()
    {
        return isOn;
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
                    TroubleshootManager.Instance.ShowMessage("Cannot operate AVR, motherboard not installed.", true);
                    return;
                }

                // ✅ Validate slots
                foreach (ItemSlot slot in motherboard.itemSlots)
                {
                    if (slot.currentItem == null)
                    {
                        TroubleshootManager.Instance.ShowMessage($"Cannot operate AVR, {slot.name} is empty.", true);
                        return;
                    }

                    string actualName = slot.currentItem.name.Replace("(Clone)", "").Trim();
                    string expectedName = slot.expectedItemName.Trim();

                    if (!string.Equals(expectedName, actualName, System.StringComparison.OrdinalIgnoreCase))
                    {
                        TroubleshootManager.Instance.ShowMessage(
                            $"Cannot operate AVR, {slot.name} has wrong item (expected {expectedName}, got {actualName}).",
                            true
                        );
                        return;
                    }
                }

                // ✅ VGA cable check
                bool vgaSystemUnit = vgaCable.IsConnectedToSystemUnit();
                bool vgaMonitor = vgaCable.IsConnectedToMonitor();

                if (!vgaSystemUnit || !vgaMonitor)
                {
                    TroubleshootManager.Instance.ShowMessage("Cannot operate AVR, VGA cable not fully connected.", true);
                    return;
                }

                // ✅ Toggle AVR
                if (!isOn)
                {
                    isOn = true;
                    if (lightsOnSprite != null && spriteRenderer != null)
                        spriteRenderer.sprite = lightsOnSprite;

                    // ✅ Monitor ON
                    if (monitorRenderer != null && monitorOnSprite != null)
                        monitorRenderer.sprite = monitorOnSprite;

                    TroubleshootManager.Instance.ShowMessage("AVR is turned ON, all conditions met: Hardware Assembly Complete.", false);
                }
                else
                {
                    if (systemUnit.IsMotherboardInstalled())
                    {
                        isOn = false;
                        if (lightsOffSprite != null && spriteRenderer != null)
                            spriteRenderer.sprite = lightsOffSprite;

                        // ✅ Monitor OFF
                        if (monitorRenderer != null && monitorOffSprite != null)
                            monitorRenderer.sprite = monitorOffSprite;

                        TroubleshootManager.Instance.ShowMessage("AVR has been turned OFF.", false);
                    }
                    else
                    {
                        TroubleshootManager.Instance.ShowMessage("Cannot turn off AVR, motherboard not installed.", true);
                    }
                }
            }
        }
    }
}
