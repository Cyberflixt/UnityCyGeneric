using System;
using UnityEngine;

public class CollisionEventProxy : MonoBehaviour
{
    public Action<Collision> onCollisionEnter;
    public Action<Collision> onCollisionExit;
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        onCollisionEnter?.Invoke(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        onCollisionExit?.Invoke(collision);
    }
}
