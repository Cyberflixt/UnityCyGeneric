using UnityEngine;

public class LagOnStart : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.5f;
    
    void Start()
    {
        LagManager.Lag(duration);
    }
}
