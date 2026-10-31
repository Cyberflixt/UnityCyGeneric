using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Uses buttons to make an input axis
/// </summary>
public class UiButtonsHoldAxisGroup : MonoBehaviour
{
    [SerializeField] private UiButtonsHoldAxis[] buttonsHold;

    public Vector2 axis
    {
        get
        {
            Vector2 sum = Vector2.zero;
            foreach (UiButtonsHoldAxis btn in buttonsHold)
            {
                if (btn.down)
                {
                    sum += btn.axis;
                }
            }
            return sum;
        }
    }
}
