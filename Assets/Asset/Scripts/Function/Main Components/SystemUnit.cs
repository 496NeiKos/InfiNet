using UnityEngine;

public class SystemUnit : MonoBehaviour
{
    private bool motherboardInstalled = false;

    // Toggle install/eject when motherboard is dragged onto the System Unit
    public void ToggleMotherboard()
    {
        if (!motherboardInstalled)
        {
            motherboardInstalled = true;
            TroubleshootManager.Instance.ShowMessage("Motherboard installed in system unit.", false);
        }
        else
        {
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
