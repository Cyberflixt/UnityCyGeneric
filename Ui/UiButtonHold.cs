using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UiButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

    private bool down = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        down = true;
        OnPressed?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (down)
        {
            down = false;
            OnReleased?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (down)
        {
            down = false;
            OnReleased?.Invoke();
        }
    }
}
