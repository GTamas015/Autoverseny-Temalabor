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

    public Vector3[] quad = new Vector3[2];

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
            wheelFL.motorTorque = maxMotorTorque * (1 - wheelFL.steerAngle/100);
            wheelFR.motorTorque = maxMotorTorque * (1 - wheelFR.steerAngle/100);
        }

    }

    private void ChechWaypointDistance()
    {
        //if (Vector3.Distance(transform.position, nodes[currentNode].position) < distanceRadius)
        //{
        //    if (currentNode == nodes.Count - 1)
        //    {
        //        currentNode = 0;
        //    }
        //    else
        //    {
        //        currentNode++;
        //    }
        //}
        quad[0] = new Vector3(nodes[currentNode].position.x - (nodes[currentNode].localScale.x / 2),
            transform.position.y,
            nodes[currentNode].position.z - (nodes[currentNode].localScale.y / 2));
        quad[1] = new Vector3(nodes[currentNode].position.x + (nodes[currentNode].localScale.x / 2),
            transform.position.y,
            nodes[currentNode].position.z - (nodes[currentNode].localScale.y / 2));

        //egyenes egyenlete (y2-y1)*(x-x1) = (x2-x1)*(y-y1)
        float x21 = quad[1].x - quad[0].x;
        float z21 = quad[1].z - quad[0].z;

        if (minDistance(transform.position,quad[0],quad[1]) < distanceRadius ) {
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

    private float minDistance(Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 d = (C - B) / (Vector3.Distance(C, B));
        Vector3 v = A - B;
        float t = Vector3.Dot(v, d);
        Vector3 P = B + t * d;
        return Vector3.Distance(P, A);
    }
}

