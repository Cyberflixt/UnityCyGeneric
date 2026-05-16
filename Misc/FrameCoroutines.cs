using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WaitOneFrame { }

/// <summary>
/// Unity coroutines are ass, use this instead with yield return new WaitOneFrame()
/// </summary>
public class FrameCoroutines : MonoBehaviour
{
    private class FrameCoroutine
    {
        private Stack<IEnumerator> enumatorStack;

        public FrameCoroutine(IEnumerator routine)
        {
            enumatorStack = new Stack<IEnumerator>();
            enumatorStack.Push(routine);
        }

        /// <summary>
        /// Updates and returns false if finished
        /// </summary>
        public bool Update()
        {
            if (enumatorStack.Count == 0)
                return false;

            IEnumerator top = enumatorStack.Peek();

            // Advance until we must yield
            if (!top.MoveNext())
            {
                // End of enumerator, remove a level
                enumatorStack.Pop();
                return Update();
            }

            var yielded = top.Current;

            // Ignore nulls
            if (yielded is null)
                return Update();

            // Run nested IEnumerator now
            if (yielded is IEnumerator nested)
            {
                enumatorStack.Push(nested);
                return Update();
            }

            // Explicit one-frame wait
            if (yielded is WaitOneFrame)
            {
                return true;
            }
            else
            {
                Debug.LogWarning("Unknown behavior: Timed routine yielded non supported type: " + yielded);
                return true;
            }
        }
    }

    private static Queue<FrameCoroutine> routines = new Queue<FrameCoroutine>();
    
    /// <summary>
    /// Starts a FrameCoroutine
    /// </summary>
    /// <param name="routine"></param>
    public static new void StartCoroutine(IEnumerator routine)
    {
        FrameCoroutine coroutine = new FrameCoroutine(routine);
        if (coroutine.Update())
            routines.Enqueue(coroutine);
    }

    /// <summary>
    /// Updates all running coroutines
    /// </summary>
    private void Update()
    {
        for (int i = 0; i < routines.Count; i++)
        {
            FrameCoroutine coroutine = routines.Dequeue();
            bool running = coroutine.Update();
            if (running)
            {
                routines.Enqueue(coroutine);
            }
        }
    }

    /// <summary>
    /// Use this instead of WaitForSeconds if you're using a FrameCoroutine
    /// </summary>
    public static IEnumerator WaitForSeconds(float seconds)
    {
        float startTime = Time.time;
        while (Time.time < startTime + seconds)
        {
            yield return new WaitOneFrame();
        }
    }
}