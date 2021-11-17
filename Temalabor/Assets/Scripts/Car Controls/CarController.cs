using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private bool isBreaking;
    private float currentBrakeForce;

    [SerializeField] public float MotorForce;
    [SerializeField] public float BrakeForce;
    [SerializeField] public float maxSteeringAngle;

    [SerializeField] public Rigidbody RB;

    [SerializeField] public float velocity;
    [SerializeField] public float maxSpeed;

    [SerializeField] public int activeEffect;
    [SerializeField] public float effectDuration;

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
        RB = GetComponent<Rigidbody>();

        Vector3 com;
        com = RB.transform.position;
        com.y += 0.2f;
        com.z += 0.35f;

        RB.centerOfMass = com;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        getInput();

        handleCar();

        handleBraking();

        steering();

        calculateVelocity();

        flipCar();

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
        if (RB.velocity.magnitude < maxSpeed)
        {
            FrontLeftCollider.motorTorque = vertical * MotorForce;
            FrontRightCollider.motorTorque = vertical * MotorForce;
            RearLeftCollider.motorTorque = vertical * MotorForce;
            RearRightCollider.motorTorque = vertical * MotorForce;
        }
        else
        {
            FrontLeftCollider.motorTorque = 0;
            FrontRightCollider.motorTorque = 0;
            RearLeftCollider.motorTorque = 0;
            RearRightCollider.motorTorque = 0;
        }
    }
    private void handleBraking()
    {
        if (isBreaking)
        {
            currentBrakeForce = BrakeForce;
        }
        else
        {
            currentBrakeForce = 0f;
        }

        RearLeftCollider.brakeTorque = currentBrakeForce;
        RearRightCollider.brakeTorque = currentBrakeForce;
    }
    private void steering()
    {
        FrontLeftCollider.steerAngle = maxSteeringAngle * horizontal;
        FrontRightCollider.steerAngle = maxSteeringAngle * horizontal;
    }
    private void calculateVelocity()
    {
        velocity = RB.velocity.magnitude;
        if (velocity < 0.001)
        {
            velocity = 0;
        }
    }
    private void flipCar()
    {
        if (RB.transform.rotation.eulerAngles.z >= 10 && velocity == 0)
        {
            Vector3 extraHeight = RB.transform.position;
            extraHeight.y += 0.5f;
            RB.transform.position = extraHeight;

            float r = RB.transform.rotation.eulerAngles.z;

            RB.transform.Rotate(0, 0, 360 - r);
        }
    }
    private void updateWheel(WheelCollider c, Transform t)
    {
        Vector3 _position;
        Quaternion _rotation;
        c.GetWorldPose(out _position, out _rotation);
        t.position = _position;
        t.rotation = _rotation;
    }
    private void updateAllWheels()
    {
        updateWheel(FrontLeftCollider, FrontLeftTransform);
        updateWheel(FrontRightCollider, FrontRightTransform);
        updateWheel(RearLeftCollider, RearLeftTransform);
        updateWheel(RearRightCollider, RearRightTransform);
    }
    private void effect1()
    {

    }
    private void effect2()
    {
        
    }
}
