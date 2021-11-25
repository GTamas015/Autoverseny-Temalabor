using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControls : MonoBehaviour
{
    public Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;

    public float maxSteerAngle = 60f;  //45f
    public float maxMotorTorque = 80f;
    public float currentSpeed;
    public float maxSpeed = 100f;

    public float distanceRadius = 5f;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;

    void Start()
    {
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

        Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(nodes[currentNode].position.x, nodes[currentNode].position.z));
        if (wheelFL.steerAngle == 0 || wheelFR.steerAngle == 0)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        }
        else if ((transform.position.y + 1.0f) < nodes[currentNode].position.y)
        {
            wheelFL.motorTorque = 3*maxMotorTorque;
            wheelFR.motorTorque = 3*maxMotorTorque;
        }
        else
        {
            wheelFL.motorTorque = maxMotorTorque / (wheelFL.steerAngle*1 + 2f);
            wheelFR.motorTorque = maxMotorTorque / (wheelFR.steerAngle*1 + 2f);
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
            }
        }
    }
}

