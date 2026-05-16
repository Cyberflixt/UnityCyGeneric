using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoneKinematics : MonoBehaviour
{
    public abstract void UpdateBone();
    public abstract void UpdateBoneVisuals();

    public void UpdateKinematics(){
        UpdateBone();
        
        foreach (BoneKinematics child in children){
            child.UpdateKinematics();
        }
    }
    public void UpdateKinematicsVisuals(){
        UpdateBoneVisuals();
        
        foreach (BoneKinematics child in children){
            child.UpdateKinematicsVisuals();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (isRoot){
            UpdateKinematics();
        }
    }
    protected virtual void LateUpdate()
    {
        if (isRoot){
            UpdateKinematicsVisuals();
        }
    }

    private bool isRoot = true;
    private BoneKinematics[] children = new BoneKinematics[0];
    protected virtual void Start()
    {
        if (transform.parent.TryGetComponent(out BoneKinematics parent)){
            isRoot = false;
        }

        List<BoneKinematics> found = new List<BoneKinematics>();
        foreach (Transform child in transform){
            if (child.TryGetComponent(out BoneKinematics childJiggle)){
                found.Add(childJiggle);
            }
        }
        children = found.ToArray();

    }
}
