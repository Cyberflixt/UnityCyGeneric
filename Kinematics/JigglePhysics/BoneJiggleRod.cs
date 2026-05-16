using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class BoneJiggleRod : BoneKinematics
{
    [SerializeField] private Transform tip;

    [Header("Settings")]
    [SerializeField] private float damp = 5f;
    [SerializeField] private float stiffnessStrength = 0f;
    [SerializeField] private float stiffnessExponent = 1f;
    [SerializeField] private float timescale = 1f;
    [SerializeField] private Vector3 gravity = new Vector3(0, -1f, 0);

    [Tooltip("Usually the character root")]
    [SerializeField] private Transform steadyReferenceFrame;
    [SerializeField] private RotationSolver rotationSolver = RotationSolver.projection;

    [Header("Constraints")]
    [SerializeField] private bool limits = false;
    [SerializeField, ShowIf("limits")] private float limitDeg = 90;


    private enum RotationSolver
    {
        projection,
        relative,
    }


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

        if (steadyReferenceFrame == null)
            steadyReferenceFrame = transform.parent;
    }


    private Quaternion rotationTarget;
    public override void UpdateBone()
    {
        tipVelocity /= 1 + Time.deltaTime * damp * 5;

        UpdateRodGravity();
        
        if (stiffnessStrength > 0){
            ConstraintRodStiffnessForce();
        }

        tipPos += tipVelocity * timescale;
        ConstraintStrictDistance();
        ConstraintAngularVelocity();

        // Set rotation
        rotationTarget = CalculateFinalRotation();
        transform.rotation = rotationTarget;
    }

    public void UpdateRodGravity()
    {
        Vector3 elasticDir = (transform.position - tipPos).normalized;
        float gravStrength = gravity.magnitude;

        Vector3 gravForce = gravity - elasticDir * Vector3.Dot(elasticDir, gravityDir);
        tipVelocity += gravForce * gravStrength * Time.deltaTime * .5f;
    }

    private void ConstraintRodStiffnessForce(){
        Vector3 elasticDir = (tipPos - transform.position).normalized;
        Vector3 targetDir = transform.parent.rotation * (initialLocalRotation *  Vector3.up);

        float dot = Vector3.Dot(elasticDir, targetDir);
        Vector3 stiffnessDir = (targetDir - dot * elasticDir).normalized;

        float distStrength = (1 - dot) / 2;
        if (stiffnessExponent != 1)
            distStrength = Mathf.Pow(distStrength, stiffnessExponent);

        tipVelocity += stiffnessDir * stiffnessStrength * distStrength * Time.deltaTime;
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
        //Vector3 tipTargetProjection = steadyReferenceFrame.rotation * tipTargetProjectionLocal + transform.position;

        DebugPlus.DrawLine(transform.position, tipTargetProjection);

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
    
    private Quaternion CalculateFinalRotation()
    {
        if (limits)
        {
            return ConstraintAngle();
        }
        else
        {
            return CalculateBoneRotation();
        }
    }

    private Quaternion CalculateBoneRotation()
    {
        if (rotationSolver == RotationSolver.projection)
        {
            return CalculateBoneRotationProjection();
        }
        else
        {
            return CalculateBoneRotationRelative();
        }
    }
    
    private Quaternion CalculateBoneRotationRelative()
    {
        // Direction to align the bone's up axis with
        Vector3 targetUp = (tipPos - transform.position).normalized;

        // Convert local axes to world space
        Vector3 worldLocalUp = transform.TransformDirection(Vector3.up);

        // Calculate rotation that aligns worldLocalUp to targetUp
        Quaternion alignUp = Quaternion.FromToRotation(worldLocalUp, targetUp);

        // Apply rotation to transform
        Quaternion newRotation = alignUp * transform.rotation;

        return newRotation;
    }

    private Quaternion CalculateBoneRotationProjection()
    {
        Vector3 direction = (tipPos - transform.position).normalized;

        // The direction the up vector should point to
        Vector3 upVector = direction;

        // Pick a consistent forward direction (parent.forward or world.forward)
        Vector3 referenceForward = transform.parent ? transform.parent.forward : Vector3.forward;

        // Remove any component of referenceForward in the direction of upVector
        referenceForward = Vector3.ProjectOnPlane(referenceForward, upVector).normalized;

        // Build the rotation
        Quaternion rotation = Quaternion.LookRotation(referenceForward, upVector);

        return rotation;
    }

    private Quaternion rotationAnim;
    public override void UpdateBoneVisuals()
    {
        rotationAnim = Quaternion.Lerp(rotationAnim, rotationTarget, Time.deltaTime * 10);
        transform.rotation = rotationAnim;
    }

    /*
    private Quaternion CalculateBoneRotation1()
    {
        // 1. Direction for the up vector
        Vector3 direction = (tipPos - transform.position).normalized;

        // 2. Reference forward vector (perpendicular to up)
        // We'll use transform.forward projected onto a plane perpendicular to direction.
        Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, direction).normalized;

        // If the projectedForward is too small (almost parallel to direction), pick an arbitrary vector.
        if (projectedForward == Vector3.zero)
        {
            projectedForward = Vector3.Cross(direction, Vector3.right);
            if (projectedForward == Vector3.zero)
                projectedForward = Vector3.Cross(direction, Vector3.up);
        }

        // 3. Construct the rotation
        Quaternion rotation = Quaternion.LookRotation(projectedForward, direction);

        return rotation;
    }
    */
}
