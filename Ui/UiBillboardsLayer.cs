using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class UiBillboardsLayer : MonoBehaviour
{
    public static UiBillboardsLayer instance;

    private static Dictionary<Transform, Transform> billboards = new Dictionary<Transform, Transform>();
    private static Dictionary<Transform, Transform> billboardsPermanent = new Dictionary<Transform, Transform>();
    public DamagePopup prefabDamagePopup = null;

    private static void Project(Vector3 world, Transform bb)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(world);
        bb.position = new Vector3(screenPos.x, screenPos.y, 0);
    }

    public static void Attach(Transform worldObject, Transform uiBillboard)
    {
        if (uiBillboard.parent != instance.transform)
            uiBillboard.SetParent(instance.transform);
        billboards[worldObject] = uiBillboard;
        Project(worldObject.position, uiBillboard);
    }

    public static void AttachPermanent(Transform worldObject, Transform uiBillboard)
    {
        if (uiBillboard.parent != instance.transform)
            uiBillboard.SetParent(instance.transform);
        billboardsPermanent[worldObject] = uiBillboard;
        Project(worldObject.position, uiBillboard);
    }

    public static void Attach(Vector3 worldPos, Transform uiBillboard)
    {
        uiBillboard.SetParent(instance.transform);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        uiBillboard.position = new Vector3(screenPos.x, screenPos.y, 0);
        Project(worldPos, uiBillboard);
    }

    public static void RemoveBillboards(Transform worldObject)
    {
        Destroy(billboards[worldObject].gameObject);
        billboards.Remove(worldObject);
    }

    public static void CreateDamagePopup(Vector3 pos, string damage)
    {
        DamagePopup popup = Instantiate(instance.prefabDamagePopup);
        Attach(pos, popup.transform);
        popup.SetText(damage);
    }

    void Start()
    {
        instance = this;
    }


    void LateUpdate()
    {
        // Update attached billboards
        List<Transform> toDelete = new List<Transform>();
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

        // Permanent billboards
        foreach (KeyValuePair<Transform, Transform> kv in billboardsPermanent)
            Project(kv.Key.position, kv.Value);
    }
}
