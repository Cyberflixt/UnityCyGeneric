using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiNavigationNode : MonoBehaviour
{
    public string soundName;

    public virtual void Selected()
    {
        Debug.Log(gameObject);
        if (soundName != "")
            Sounds.PlayAudioFlat(SoundsType.SFX, soundName, 1, .1f);
    }
    public virtual void Deselected()
    {
        
    }

    private void OnDisable()
    {
        UiNavigationHandler.UndoUntilNotNull();
    }
}
