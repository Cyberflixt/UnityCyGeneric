using System.Collections;
using UnityEngine;

public class UiFlash : MonoBehaviour
{
    public GameObject flashInstance;
    private static UiFlash instance;

    private void Awake()
    {
        instance = this;
        flashInstance.SetActive(false);
    }

    public static void Flash(float duration)
    {
        instance.flashInstance.SetActive(true);
        instance.StartCoroutine(FlashCoroutine(duration));
    }

    public static IEnumerator FlashCoroutine(float duration)
    {
        instance.flashInstance.SetActive(true);
        yield return new WaitForSeconds(duration);
        instance.flashInstance.SetActive(false);
    }
}
