using UnityEngine;

public class HoverTest : MonoBehaviour
{
    private void OnMouseEnter()
    {
        if (HoverLabelManager.Instance != null)
        {
            HoverLabelManager.Instance.ShowLabel(gameObject.name);
            Debug.Log("Hovering over " + gameObject.name);
        }
    }

    private void OnMouseExit()
    {
        if (HoverLabelManager.Instance != null)
        {
            HoverLabelManager.Instance.HideLabel();
            Debug.Log("Stopped hovering " + gameObject.name);
        }
    }
}
