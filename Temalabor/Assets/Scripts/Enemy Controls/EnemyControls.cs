using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControls : MonoBehaviour
{
    public Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;

    private float maxSteerAngle = 90f;  //45f
    private float maxMotorTorque = 600f;
    public float currentSpeed;
    public float maxSpeed = 100f;

    private float distanceRadius = 5.0f;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider RearLeftCollider;
    public WheelCollider RearRightCollider;
    public float ero;
    public float Seged1;
    public float Seged2;
    public float rising = 0;
    public float slope = 0;
    public float once = 0;

    public Vector3[] quad = new Vector3[2];

    void Start()
    {
        ero = RearLeftCollider.motorTorque;
        Transform[] pathTranforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTranforms.Length; i++)
        {
            if (pathTranforms[i] != path.transform)
            {
                nodes.Add(pathTranforms[i]);
            }
        }
    }

    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        ChechWaypointDistance();
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
        Seged1 = transform.position.y;
        Seged2 = nodes[currentNode].position.y;
        ero = Mathf.Abs(wheelFL.steerAngle);

        if (rising > 0)
        {
            wheelFL.motorTorque = maxMotorTorque * 3;
            wheelFR.motorTorque = maxMotorTorque * 3;
            RearLeftCollider.motorTorque = maxMotorTorque * 3;
            RearRightCollider.motorTorque = maxMotorTorque * 3;
        }
        else if (slope > 0)
        {
            wheelFL.motorTorque = maxMotorTorque / 5;
            wheelFR.motorTorque = maxMotorTorque / 5;
            RearLeftCollider.motorTorque = maxMotorTorque / 3;
            RearRightCollider.motorTorque = maxMotorTorque / 3;
        }
        else if (Mathf.Abs(wheelFL.steerAngle) < 30.0f || Mathf.Abs(wheelFR.steerAngle) < 30.0f)
        {
            wheelFL.motorTorque = maxMotorTorque * 2;
            wheelFR.motorTorque = maxMotorTorque * 2;
            RearLeftCollider.motorTorque = 0;
            RearRightCollider.motorTorque = 0;
        }
        else
        {
            wheelFL.motorTorque = maxMotorTorque / 8;
            wheelFR.motorTorque = maxMotorTorque / 8;
            RearLeftCollider.motorTorque = maxMotorTorque / 8;
            RearRightCollider.motorTorque = maxMotorTorque / 8;
        }

    }

    private void ChechWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < distanceRadius)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;

                // emelkedo beallitasa
                if (rising == 0)
                {
                    if (nodes[currentNode+1].position.y > transform.position.y)
                    {
                        rising = 3;
                    }
                }
                if (rising > 0)
                {
                    rising--;
                }

                // leejto beallitasa
                if (once == 0)
                {
                    if (slope == 0)
                    {
                        if (nodes[currentNode + 1].position.y + 3.0f < transform.position.y)
                        {
                            slope = 2;
                            once = 1;
                        }
                    }
                }
                if (slope > 0)
                {
                    slope--;
                }
            }
        }
    }

}

