using System.Collections;
using UnityEngine;

public class DisableAfter : MonoBehaviour
{
    public float delay = 3f;

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(delay);
        gameObject.SetActive(false);
    }
}
