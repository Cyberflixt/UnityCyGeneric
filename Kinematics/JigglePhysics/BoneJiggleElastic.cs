using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class BoneJiggleElastic : BoneKinematics
{
    [SerializeField] private Transform tip;

    [Header("Settings")]
    [SerializeField] private float damp = 5f;
    [SerializeField] private float elasticStrength = 1f;
    [SerializeField] private float stiffnessStrength = 0f;
    [SerializeField] private float timescale = 1f;
    [SerializeField] private Vector3 gravity = new Vector3(0, -.2f, 0);

    [Header("Constraints")]
    //[WarningBox("Strict distance only forces the radius to stay constant, the velocity and real position will still behave normally! Consider using BoneJiggleRod instead.")]
    //[SerializeField, ShowIf("strictDistance")] private bool lorem = false;
    
    [SerializeField, Tooltip("Strict distance only forces the radius to stay constant, the velocity and real position will still behave normally! Consider using BoneJiggleRod instead.")]
    private bool strictDistance = true;
    [SerializeField] private bool limits = false;
    [SerializeField, ShowIf("limits")] private float limitDeg = 90;

    [Header("Advanced")]
    [SerializeField] private Vector3 localUpAxis = Vector3.up; // Axis towards next bone
    //[SerializeField] private bool debug = false; // Axis towards next bone
    //[SerializeField] private Vector3 localRollAxis = Vector3.forward; // Axis to keep fixed


    private Vector3 tipVelocity = Vector3.zero;
    private Vector3 tipPos;
    
    private float initialDistance;
    private Quaternion initialLocalRotation;
    private Quaternion initialRotation;
    private Vector3 gravityDir;


    protected override void Start()
    {
        base.Start();

        tipPos = tip.position;
        initialDistance = (transform.position - tip.position).magnitude;
        initialLocalRotation = transform.localRotation;
        initialRotation = transform.rotation;
        gravityDir = gravity.normalized;
    }



    public override void UpdateBone()
    {
        tipVelocity /= 1 + Time.deltaTime * damp * .1f;

        tipVelocity += gravity * Time.deltaTime;

        float distance = (tipPos - transform.position).magnitude;
        Vector3 elasticDir = (transform.position - tipPos).normalized;
        tipVelocity += elasticDir * (distance - initialDistance) * .01f * elasticStrength;
        
        if (stiffnessStrength > 0){
            ConstraintStiffnessForce();
        }

        tipPos += tipVelocity * timescale;

        if (strictDistance){
            ConstraintStrictDistance();
        }
        ConstraintAngle();

        // Set rotation
        transform.rotation = CalculateFinalRotation();
    }

    private void ConstraintStiffnessForce(){
        Vector3 tipTargetProjectionLocal = initialLocalRotation *  (Vector3.up * initialDistance);
        Vector3 tipTargetProjection = transform.parent.rotation * tipTargetProjectionLocal + transform.position;

        float distance = (tipPos - tipTargetProjection).magnitude;
        Vector3 elasticDir = (tipTargetProjection - tipPos).normalized;
        
        tipVelocity +=  elasticDir * distance * Mathf.Min(Time.deltaTime * stiffnessStrength, 1);
    }
    
    private void ConstraintStrictDistance(){
        // Correct tip position
        float distance = (tipPos - transform.position).magnitude;
        Vector3 elasticDir = (transform.position - tipPos).normalized;

        tipPos += elasticDir * (distance - initialDistance);
    }

    private void ConstraintAngularVelocity(){
        // Get polar vectors
        Vector3 elasticDir = (transform.position - tipPos).normalized;

        Vector3 crossA = Vector3.Cross(elasticDir, Vector3.forward).normalized;
        Vector3 crossB = Vector3.Cross(elasticDir, crossA).normalized;
        
        // Project velocity to plane opposed to bone direction
        float dotA = Vector3.Dot(crossA, tipVelocity);
        float dotB = Vector3.Dot(crossB, tipVelocity);

        tipVelocity = dotA * crossA + dotB * crossB;
    }

    private Quaternion ConstraintAngle(){
        Vector3 tipTargetProjectionLocal = initialLocalRotation *  (Vector3.up * initialDistance);
        Vector3 tipTargetProjection = transform.parent.rotation * tipTargetProjectionLocal + transform.position;

        Vector3 direction = (tipPos - transform.position).normalized;
        Vector3 targetDir = (tipTargetProjection - transform.position).normalized;

        float dot = Vector3.Dot(direction, targetDir);
        float dotLimit = Mathf.Cos(limitDeg);

        Quaternion boneRotation = CalculateBoneRotation();

        if (dot < dotLimit){
            //return boneRotation;
        } else {
            Quaternion projectedInitialRotation = transform.parent.rotation * initialRotation;
            //return projectedInitialRotation;
            return Quaternion.RotateTowards(projectedInitialRotation, boneRotation, limitDeg);
        }
        return transform.parent.rotation * initialLocalRotation;
    }

    private Quaternion CalculateBoneRotation()
    {
        // Direction to align the bone's up axis with
        Vector3 targetUp = (tipPos - transform.position).normalized;

        // Convert local axes to world space
        Vector3 worldLocalUp = transform.TransformDirection(localUpAxis);

        // Calculate rotation that aligns worldLocalUp to targetUp
        Quaternion alignUp = Quaternion.FromToRotation(worldLocalUp, targetUp);

        // Apply rotation to transform
        Quaternion newRotation = alignUp * transform.rotation;

        return newRotation;
    }

    private Quaternion CalculateFinalRotation()
    {
        if (limits){
            return ConstraintAngle();
        } else {
            return CalculateBoneRotation();
        }
    }

    public override void UpdateBoneVisuals()
    {
        throw new NotImplementedException();
    }
}
