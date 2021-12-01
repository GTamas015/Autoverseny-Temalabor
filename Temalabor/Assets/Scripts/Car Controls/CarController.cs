using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private bool isBraking;

    private float originalMaxSpeed;
    private WheelFrictionCurve originalfc;
    private float originalAirDragValue;

    private bool CoinEffectActive;
    private bool BananaEffectActive;

    [SerializeField] public float MotorForce;
    [SerializeField] public float BrakeForce;
    [SerializeField] public float maxSteeringAngle;
    [SerializeField] public float DecelerationForce;
    [SerializeField] public float AirDragValue;

    [SerializeField] public Rigidbody RB;

    [SerializeField] public float velocity;
    [SerializeField] public float maxSpeed;

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

        CoinEffectActive = false;
        BananaEffectActive = false;

        originalMaxSpeed = maxSpeed;
        originalfc = FrontLeftCollider.sidewaysFriction;
        originalAirDragValue = AirDragValue;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        getInput();

        handleCar();

        handleBraking();

        steering();

        calculateVelocity();

        airDrag();

        handleBraking();

        flipCar();

        updateAllWheels();
    }

    private void getInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void handleCar()
    {
        FrontLeftCollider.motorTorque = vertical * MotorForce;
        FrontRightCollider.motorTorque = vertical * MotorForce;
        RearLeftCollider.motorTorque = vertical * MotorForce;
        RearRightCollider.motorTorque = vertical * MotorForce;
    }
    private void handleBraking()
    {
        if (isBraking)
        {
            RearLeftCollider.brakeTorque += BrakeForce;
            RearRightCollider.brakeTorque += BrakeForce;
        }
    }
    private void steering()
    {
        FrontLeftCollider.steerAngle = maxSteeringAngle * horizontal;
        FrontRightCollider.steerAngle = maxSteeringAngle * horizontal;
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
    public void coinEffect()
    {
        if (!CoinEffectActive)
        {
            CoinEffectActive = true;
            AirDragValue = originalAirDragValue / 2.0f;
            Invoke("reverseCoinEffect", effectDuration);
        }
    }

    private void reverseCoinEffect()
    {
        AirDragValue = originalAirDragValue;
        CoinEffectActive = false;
    }

    public void bananaEffect()
    {
        if (!BananaEffectActive)
        {
            BananaEffectActive = true;

            WheelFrictionCurve fc = FrontLeftCollider.sidewaysFriction;
            fc.stiffness = 0.8f;
            FrontLeftCollider.sidewaysFriction = fc;
            FrontRightCollider.sidewaysFriction = fc;
            RearLeftCollider.sidewaysFriction = fc;
            RearRightCollider.sidewaysFriction = fc;
            Debug.Log("fc");
            Invoke("reverseBananaEffect", effectDuration);
            Debug.Log("reverseover");
        }
    }
    private void reverseBananaEffect()
    {
        FrontLeftCollider.sidewaysFriction = originalfc;
        FrontRightCollider.sidewaysFriction = originalfc;
        RearLeftCollider.sidewaysFriction = originalfc;
        RearRightCollider.sidewaysFriction = originalfc;
        Debug.Log("reverse");
        BananaEffectActive = false;
    }

    private void airDrag()
    {
        float drag;

        if (velocity > 15.0f)
        {
            drag = MotorForce * (velocity / maxSpeed) * AirDragValue;

            if (drag < 0.1f)
                drag = 0.0f;

            FrontLeftCollider.brakeTorque = drag;
            FrontRightCollider.brakeTorque = drag;
            RearRightCollider.brakeTorque = drag;
            RearLeftCollider.brakeTorque = drag;
        }
        else
        {
            drag = 0.0f * AirDragValue;

            FrontLeftCollider.brakeTorque = drag;
            FrontRightCollider.brakeTorque = drag;
            RearRightCollider.brakeTorque = drag;
            RearLeftCollider.brakeTorque = drag;
        }
    }
}