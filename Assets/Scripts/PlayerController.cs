using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;
    [SerializeField]

    private float thrusterForce = 1000f;
    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;
    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFuelAmount = 1f;

    [SerializeField]
    private LayerMask environmentMask;

    [Header("Thruster Settings")]
    //[SerializeField]
    //private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    //Component caching
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        motor = GetComponent<PlayerMotor>();
        animator = GetComponent<Animator>();

        setJointSettings(jointSpring);
    }

    void Update()
    {
        if (PauseMenu.isOn)
        {
            if(Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            motor.move(Vector3.zero);
            motor.rotate(Vector3.zero);
            motor.rotateCamera(0f);
            motor.applyThruster(Vector3.zero);
            return;
        }

        if(Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        //Setting target position for spring/thruster correcting the physics of gravity when flying over other objects
        RaycastHit groundHit;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 100f, environmentMask))
        {
            joint.targetPosition = new Vector3(0f, -groundHit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }

        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * zMov;

        Vector3 velocityParam = (movHorizontal + movVertical) * speed;

        animator.SetFloat("ForwardVelocity", zMov);

        motor.move(velocityParam);

        //rotation around the y axis
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotationParam = new Vector3(0f, yRot, 0f) * lookSensitivity;

        motor.rotate(rotationParam);

        //camera rotation as 3d vector around x
        float xRot = Input.GetAxisRaw("Mouse Y");
        float cameraRotationParamX = xRot * lookSensitivity;

        motor.rotateCamera(cameraRotationParamX);

        //calc thruster force based off user
        Vector3 thrusterForceTemp = Vector3.zero;

        //if we're flying
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            if(thrusterFuelAmount >= 0.01f)
            {
                thrusterForceTemp = Vector3.up * thrusterForce;
                setJointSettings(0f);
            }
        }
        //else we're not flying
        else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;

            setJointSettings(jointSpring);
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

        motor.applyThruster(thrusterForceTemp);
    }

    public float getThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }

    private void setJointSettings(float jointSpringTemp)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = jointSpring,
            maximumForce = jointMaxForce
        };
    }
}
