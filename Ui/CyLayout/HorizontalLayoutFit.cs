using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class HorizontalLayoutFit : CyLayoutBase
{
    public float margin = 0;
    public Transform list;

    public override void RefreshLayout()
    {
        // Get children total width
        float width = 0;
        foreach (RectTransform child in list)
        {
            if (child.gameObject.activeSelf)
                width += child.rect.width;
        }

        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width + margin, rect.sizeDelta.y);
    }
}
