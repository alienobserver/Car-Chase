using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    private Rigidbody rb;
    private float AntiRoll = 15000f;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        FixCarRotation();
    }

    private void FixCarRotation()
    {
        WheelHit hit;
        float travelFL = 1f;
        float travelFR = 1f;
        float travelRL = 1f;
        float travelRR = 1f;

        bool groundedFL = frontLeftWheelCollider.GetGroundHit(out hit);
        if (groundedFL)
        {
            travelFL = (-frontLeftWheelCollider.transform.InverseTransformPoint(hit.point).y - frontLeftWheelCollider.radius) / frontLeftWheelCollider.suspensionDistance;
        }

        bool groundedFR = frontRightWheelCollider.GetGroundHit(out hit);
        if (groundedFR)
        {
            travelFR = (-frontRightWheelCollider.transform.InverseTransformPoint(hit.point).y - frontRightWheelCollider.radius) / frontRightWheelCollider.suspensionDistance;
        }

        bool groundedRL = rearLeftWheelCollider.GetGroundHit(out hit);
        if (groundedFL)
        {
            travelRL = (-rearLeftWheelCollider.transform.InverseTransformPoint(hit.point).y - rearLeftWheelCollider.radius) / rearLeftWheelCollider.suspensionDistance;
        }

        bool groundedRR = rearRightWheelCollider.GetGroundHit(out hit);
        if (groundedFR)
        {
            travelRR = (-rearRightWheelCollider.transform.InverseTransformPoint(hit.point).y - rearRightWheelCollider.radius) / rearRightWheelCollider.suspensionDistance;
        }

        var antiRollForce = (travelFL - travelFR) * AntiRoll / 2;

        if (groundedFL)
        {
            rb.AddForceAtPosition(frontLeftWheelCollider.transform.up * -antiRollForce,
                   frontLeftWheelCollider.transform.position);
        }
        if (groundedFR)
        {
            rb.AddForceAtPosition(frontRightWheelCollider.transform.up * antiRollForce,
                   frontRightWheelCollider.transform.position);
        }

        if (groundedRL)
        {
            rb.AddForceAtPosition(rearLeftWheelCollider.transform.up * -antiRollForce,
                   rearLeftWheelCollider.transform.position);
        }
        if (groundedRR)
        {
            rb.AddForceAtPosition(rearRightWheelCollider.transform.up * antiRollForce,
                   rearRightWheelCollider.transform.position);
        }
    }

    private void ForceBreak()
    {
        frontRightWheelCollider.brakeTorque = Mathf.Infinity;
        frontLeftWheelCollider.brakeTorque = Mathf.Infinity;
        rearLeftWheelCollider.brakeTorque = Mathf.Infinity;
        rearRightWheelCollider.brakeTorque = Mathf.Infinity;
        rb.velocity = Vector3.zero;
    }

    private void GetInput()
    {
        horizontalInput = 0;
        verticalInput =0;
        isBreaking = false;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
