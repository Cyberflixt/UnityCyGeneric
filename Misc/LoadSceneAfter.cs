using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfter : MonoBehaviour
{
    public float delay = 5f;
    public string sceneName = "SceneName";

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
