using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private bool isBreaking;
    private float currentBrakeForce;

    private bool effectAlreadyActive;

    private float originalMaxSpeed;
    private WheelFrictionCurve originalfc;

    private bool effect1active;
    private bool effect2active;

    [SerializeField] public float MotorForce;
    [SerializeField] public float BrakeForce;
    [SerializeField] public float maxSteeringAngle;
    [SerializeField] public float DecelerationForce;

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

        effectAlreadyActive = false;
        originalMaxSpeed = maxSpeed;
        originalfc = FrontLeftCollider.sidewaysFriction;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        getInput();

        handleCar();

        handleBraking();

        steering();

        if (!isBreaking)
        {
            decelerate();
        }

        calculateVelocity();

        flipCar();

        updateAllWheels();

        if (effect1active)
        {
            effect1();
        }

        if (effect2active)
        {
            effect2();
        }
    }

    private void getInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);

        effect1active = Input.GetKey(KeyCode.K);
        effect2active = Input.GetKey(KeyCode.L);
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
            FrontLeftCollider.motorTorque = 0.0f;
            FrontRightCollider.motorTorque = 0.0f;
            RearLeftCollider.motorTorque = 0.0f;
            RearRightCollider.motorTorque = 0.0f;
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
            currentBrakeForce = 0.0f;
        }

        RearLeftCollider.brakeTorque = currentBrakeForce;
        RearRightCollider.brakeTorque = currentBrakeForce;
    }
    private void steering()
    {
        FrontLeftCollider.steerAngle = maxSteeringAngle * horizontal;
        FrontRightCollider.steerAngle = maxSteeringAngle * horizontal;
    }
    private void decelerate()
    {
        if (vertical == 0.0f && RB.velocity.magnitude >= 15.0f)
        {
            FrontLeftCollider.brakeTorque = DecelerationForce;
            FrontRightCollider.brakeTorque = DecelerationForce;
            RearLeftCollider.brakeTorque = DecelerationForce;
            RearRightCollider.brakeTorque = DecelerationForce;
        }
        else
        {
            FrontLeftCollider.brakeTorque = 0.0f;
            FrontRightCollider.brakeTorque = 0.0f;
            RearLeftCollider.brakeTorque = 0.0f;
            RearRightCollider.brakeTorque = 0.0f;
        }

    }
    private void calculateVelocity()
    {
        velocity = RB.velocity.magnitude;
        if (velocity < 0.1f)
        {
            velocity = 0.0f;
        }
    }
    private void flipCar()
    {
        float zEulerAngle = RB.transform.rotation.eulerAngles.z;
        zEulerAngle = Mathf.Abs(zEulerAngle);
        if (zEulerAngle < 0.1f)
            zEulerAngle = 0.0f;

        if ((zEulerAngle >= 10.0f && zEulerAngle <= 355.0f) && velocity <= 0.01f)
        {
            Vector3 extraHeight = RB.transform.position;
            extraHeight.y += 0.5f;
            RB.transform.position = extraHeight;

            RB.transform.Rotate(0, 0, 360 - zEulerAngle);
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
        if (!effectAlreadyActive)
        {
            effectAlreadyActive = true;
            activeEffect = 1;
            maxSpeed += 20.0f;
            Invoke("reverseEffect1", effectDuration);
        }
    }

    private void reverseEffect1()
    {
        effectAlreadyActive = false;
        maxSpeed = originalMaxSpeed;
        activeEffect = 0;
        effect1active = false;
    }

    private void effect2()
    {
        if (!effectAlreadyActive)
        {
            effectAlreadyActive = true;
            activeEffect = 2;

            WheelFrictionCurve fc = FrontLeftCollider.sidewaysFriction;
            fc.stiffness = 0.8f;
            FrontLeftCollider.sidewaysFriction = fc;
            FrontRightCollider.sidewaysFriction = fc;
            RearLeftCollider.sidewaysFriction = fc;
            RearRightCollider.sidewaysFriction = fc;

            Invoke("reverseEffect2", effectDuration);
        }
    }
    private void reverseEffect2()
    {
        effectAlreadyActive = false;

        FrontLeftCollider.sidewaysFriction = originalfc;
        FrontRightCollider.sidewaysFriction = originalfc;
        RearLeftCollider.sidewaysFriction = originalfc;
        RearRightCollider.sidewaysFriction = originalfc;

        activeEffect = 0;
        effect2active = false;
    }
}