using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputExt : MonoBehaviour
{
    private static InputExt instance;
    [NonSerialized] public static Dictionary<string, InputAction> actions = new Dictionary<string, InputAction>();
    [NonSerialized] public static bool ready = false;

    // Private
    [SerializeField] private InputActionAsset inputAsset;
    static InputAction inputMove;

    void Awake()
    {
        // Singleton
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;

        // Initialize action dictionary
        actions = new Dictionary<string, InputAction>();
        foreach (InputActionMap map in inputAsset.actionMaps)
        {
            foreach (InputAction action in map.actions)
            {
                actions[action.name] = action;
            }
        }

        // Special actions
        inputMove = actions["Move"];

        // Event on ready
        onReady?.Invoke();
        onReady = null;
        ready = true;
    }


    private static Action onReady;
    public static void ReadyCallback(Action callback)
    {
        if (instance == null)
        {
            onReady += callback;
        }
        else
        {
            callback();
        }
    }

    public static Vector3 GetMoveVector()
    {
        Vector2 raw = inputMove.ReadValue<Vector2>();
        Vector3 move = new Vector3(raw.x, 0, raw.y);

        if (raw.x != 0 && raw.y != 0)
            return move.normalized;
        return move;
    }

    public void RumbleStart(float lowFrequency, float highFrequency, float duration)
    {
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
            StartCoroutine(RumbleEnd(duration));
        }
    }

    private IEnumerator RumbleEnd(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0, 0);
        }
    }
}