using UnityEngine;

public class SystemUnit : MonoBehaviour
{
    private bool motherboardInstalled = false;

    // Toggle install/eject when motherboard is dragged onto the System Unit
    public void ToggleMotherboard()
    {
        // ✅ AVR check before ejecting
        AVR avr = FindObjectOfType<AVR>();

        if (!motherboardInstalled)
        {
            motherboardInstalled = true;
            TroubleshootManager.Instance.ShowMessage("Motherboard installed in system unit.", false);
        }
        else
        {
            // ✅ Block ejection if AVR is ON
            if (avr != null && avr.IsOn())
            {
                TroubleshootManager.Instance.ShowMessage(
                    "Cannot eject Motherboard while AVR is turned ON. Please turn off AVR first.",
                    true
                );
                return; // stop here, motherboard stays installed
            }

            motherboardInstalled = false;
            TroubleshootManager.Instance.ShowMessage("Motherboard ejected from the system unit.", false);
        }
    }

    // Check if motherboard is currently installed
    public bool IsMotherboardInstalled()
    {
        return motherboardInstalled;
    }
}
