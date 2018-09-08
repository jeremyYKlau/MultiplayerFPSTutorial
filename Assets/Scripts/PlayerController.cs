using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;
    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Thruster Settings")]
    //[SerializeField]
    //private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        motor = GetComponent<PlayerMotor>();

        setJointSettings(jointSpring);
    }

    void Update()
    {
        float xMov = Input.GetAxis("Horizontal");
        float yMov = Input.GetAxis("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * yMov;

        Vector3 velocityParam = (movHorizontal + movVertical).normalized * speed;

        motor.move(velocityParam);

        //rotation around the y axis
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotationParam = new Vector3(0f, yRot, 0f) * lookSensitivity;

        motor.rotate(rotationParam);

        //camera rotation as 3d vector around x
        float xRot = Input.GetAxisRaw("Mouse Y");
        float cameraRotationParamX = xRot * lookSensitivity;

        motor.rotateCamera(cameraRotationParamX);

        Vector3 thrusterForceTemp = Vector3.zero;

        if (Input.GetButton("Jump"))
        {
            thrusterForceTemp = Vector3.up * thrusterForce;
            setJointSettings(0f);
        }
        else
        {
            setJointSettings(jointSpring);
        }

        motor.applyThruster(thrusterForceTemp);
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
