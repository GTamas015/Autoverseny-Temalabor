using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontal, vertical;
    private bool isBreaking;
    private float currentBreakForce;

    [SerializeField] public float force;
    [SerializeField] public float breakforce;
    [SerializeField] public float maxAngle;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public float velocity;
    [SerializeField] public Vector3 CenterOfMass;

    [SerializeField] public WheelCollider FrontLeftCollider;
    [SerializeField] public WheelCollider FrontRightCollider;
    [SerializeField] public WheelCollider RearLeftCollider;
    [SerializeField] public WheelCollider RearRightCollider;

    [SerializeField] public Transform FrontLeftTransform;
    [SerializeField] public Transform FrontRightTransform;
    [SerializeField] public Transform RearLeftTransform;
    [SerializeField] public Transform RearRightTransform;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CenterOfMass;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        getInput();

        handleCar();

        handleBreaking();

        Steering();

        calculateVelocity();

        updateAllWheels();
    }

    private void getInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void handleCar()
    {

        FrontLeftCollider.motorTorque = vertical * force;
        FrontRightCollider.motorTorque = vertical * force;
        RearLeftCollider.motorTorque = vertical * force;
        RearRightCollider.motorTorque = vertical * force;

        if (isBreaking)
        {
            currentBreakForce = breakforce;
        }
        else
        {
            currentBreakForce = 0f;
        }
    }
    private void Steering()
    {
        FrontLeftCollider.steerAngle = maxAngle * horizontal;
        FrontRightCollider.steerAngle = maxAngle * horizontal;
    }
    private void handleBreaking()
    {
        //FrontLeftCollider.brakeTorque = currentBreakForce;
        //FrontRightCollider.brakeTorque = currentBreakForce;
        RearLeftCollider.brakeTorque = currentBreakForce;
        RearRightCollider.brakeTorque = currentBreakForce;
    }
    private void UpdateWheel(WheelCollider c, Transform t)
    {
        Vector3 _position;
        Quaternion _rotation;
        c.GetWorldPose(out _position, out _rotation);
        t.position = _position;
        t.rotation = _rotation;
    }

    private void calculateVelocity()
    {
        Vector3 velocityVector = rb.velocity;
        float x_y = Mathf.Sqrt(velocityVector.x * velocityVector.x + velocityVector.y * velocityVector.y);
        float x_y_z = Mathf.Sqrt(x_y * x_y + velocityVector.z * velocityVector.z);
        velocity = x_y_z;
        if (velocity < 0.001)
            velocity = 0;
    }
    private void updateAllWheels()
    {
        UpdateWheel(FrontLeftCollider, FrontLeftTransform);
        UpdateWheel(FrontRightCollider, FrontRightTransform);
        UpdateWheel(RearLeftCollider, RearLeftTransform);
        UpdateWheel(RearRightCollider, RearRightTransform);
    }
}
