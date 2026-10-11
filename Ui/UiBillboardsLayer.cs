using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class UiBillboardsLayer : MonoBehaviour
{
    public static UiBillboardsLayer instance;

    private static Dictionary<Transform, Transform> billboards = new Dictionary<Transform, Transform>();
    private static Dictionary<Transform, Vector3> billboardsPosition = new Dictionary<Transform, Vector3>();
    public DamagePopup prefabDamagePopup = null;

    private static void Project(Transform bb, Vector3 world)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(world);
        bb.position = new Vector3(screenPos.x, screenPos.y, 0);
    }

    public static void Attach(Transform uiBillboard, Transform worldObject)
    {
        if (uiBillboard.parent != instance.transform)
            uiBillboard.SetParent(instance.transform);
        billboards[uiBillboard] = worldObject;
        Project(uiBillboard, worldObject.position);
    }

    public static void Attach(Transform uiBillboard, Vector3 worldPos)
    {
        uiBillboard.SetParent(instance.transform);
        billboardsPosition[uiBillboard] = worldPos;
        Project(uiBillboard, worldPos);
    }

    public static void AttachOnce(Transform uiBillboard, Vector3 worldPos)
    {
        uiBillboard.SetParent(instance.transform);
        Project(uiBillboard, worldPos);
    }

    public static void RemoveAttachedBillboards(Transform worldObject)
    {
        if (billboards[worldObject])
            Destroy(billboards[worldObject].gameObject);
        billboards.Remove(worldObject);
    }

    public static void CreateDamagePopup(Vector3 pos, string damage)
    {
        DamagePopup popup = Instantiate(instance.prefabDamagePopup);
        Attach(popup.transform, pos);
        popup.SetText(damage);
    }

    void Start()
    {
        instance = this;
    }

    List<Transform> toDelete = new();
    void LateUpdate()
    {
        // Update attached billboards
        toDelete.Clear();
        foreach (KeyValuePair<Transform, Transform> kv in billboards)
        {
            if (kv.Key)
                Project(kv.Key, kv.Value.position);
            else
                // Object doesnt exist anymore, delete
                toDelete.Add(kv.Key);
        }

        // Deletions
        foreach (Transform k in toDelete)
            RemoveAttachedBillboards(k);

        // Update attached billboards
        toDelete.Clear();
        foreach (KeyValuePair<Transform, Vector3> kv in billboardsPosition)
        {
            if (kv.Key)
                Project(kv.Key, kv.Value);
            else
                // Object doesnt exist anymore, delete
                toDelete.Add(kv.Key);
        }

        // Deletions
        foreach (Transform k in toDelete)
            billboardsPosition.Remove(k);
    }
}
