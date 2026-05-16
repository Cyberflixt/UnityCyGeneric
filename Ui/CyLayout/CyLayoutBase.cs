using UnityEngine;

public class CyLayoutBase : MonoBehaviour
{
    [Button]
    public void RefreshAllLayouts()
    {
        // Reach top-most layout

        CyLayoutBase topmost = this;
        Transform current = transform;
        while (current.parent)
        {
            current = current.parent;
            if (current.TryGetComponent(out CyLayoutBase layout))
            {
                topmost = layout;
            }
        }

        RefreshAllLayoutPercolate(topmost.transform);
    }

    private void RefreshAllLayoutPercolate(Transform current)
    {
        // Bottom first
        foreach (Transform child in current)
        {
            RefreshAllLayoutPercolate(child);
        }

        if (current.TryGetComponent(out CyLayoutBase layout))
        {
            layout.RefreshLayout();
        }
    }

    public virtual void RefreshLayout()
    {
        
    }
}
