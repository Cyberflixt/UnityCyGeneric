using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class UiBillboardsLayer : MonoBehaviour
{
    private readonly Dictionary<Transform, Transform> billboards = new();
    private readonly Dictionary<Transform, Vector3> billboardsPosition = new();
    private readonly Dictionary<Transform, Transform> billboardsPermanent = new();

    private void Project(Vector3 world, Transform bb)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(world);
        bb.position = new Vector3(screenPos.x, screenPos.y, 0);
    }

    public void Attach(Transform worldObject, Transform uiBillboard)
    {
        if (uiBillboard.parent != transform)
            uiBillboard.SetParent(transform);
        billboards[worldObject] = uiBillboard;
        Project(worldObject.position, uiBillboard);
    }

    public void AttachPermanent(Transform worldObject, Transform uiBillboard)
    {
        if (uiBillboard.parent != transform)
            uiBillboard.SetParent(transform);
        billboardsPermanent[worldObject] = uiBillboard;
        Project(worldObject.position, uiBillboard);
    }

    public void Attach(Vector3 worldPos, Transform uiBillboard)
    {
        uiBillboard.SetParent(transform);
        billboardsPosition[uiBillboard] = worldPos;
        Project(worldPos, uiBillboard);
    }

    public void RemoveBillboards(Transform worldObject)
    {
        Destroy(billboards[worldObject].gameObject);
        billboards.Remove(worldObject);
    }

    void LateUpdate()
    {
        // Update attached transform billboards
        List<Transform> toDelete = new();
        foreach (KeyValuePair<Transform, Transform> kv in billboards)
        {
            if (kv.Key)
                Project(kv.Key.position, kv.Value);
            else
                // Object doesnt exist anymore, delete
                toDelete.Add(kv.Key);
        }

        // Deletions
        foreach (Transform k in toDelete)
            RemoveBillboards(k);

        // Update attached position billboards
        toDelete = new();
        foreach (KeyValuePair<Transform, Vector3> kv in billboardsPosition)
        {
            if (kv.Key)
                Project(kv.Value, kv.Key);
            else
                // Object doesnt exist anymore, delete
                toDelete.Add(kv.Key);
        }

        // Deletions
        foreach (Transform k in toDelete)
        {
            billboardsPosition.Remove(k);
        }

        // Permanent billboards
        foreach (KeyValuePair<Transform, Transform> kv in billboardsPermanent)
            Project(kv.Key.position, kv.Value);
    }
}
