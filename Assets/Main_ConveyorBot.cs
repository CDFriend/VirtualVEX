using UnityEngine;
using System.Collections;
using System.Reflection;

/// <summary>
/// Subclass of vvRobotBase that uses the loaded code to operate the ConveyorBot
/// </summary>
public class Main_ConveyorBot : vvRobotBase
{
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider backLeft;
    public WheelCollider backRight;
    public GameObject arm1;
    public GameObject arm2;
    public GameObject intake;
    public GameObject ramp;
    public GameObject roof;
    public GameObject stopper;

    void Awake()
    {
        robotID_ = "Conveyor Bot";
    }

    void FixedUpdate()
    {
        bool timeUp = owner.getTimeLimit > 0 && owner.getTimeLeft <= 0 && tracker_.GetComponent<ModeTrackingScript>().disableOnTimeUp;
        //The robot should only move if the round hasn't ended
        //The only exeption is in solo untimed, which has no end.
        if (complete_ && !timeUp)
        {
            //Apply operator inputs to the proper mechanisms on the robot.
            setMotors(frontRight, backRight, motor[0]);
            setMotors(frontLeft, backLeft, motor[1]);
            if (Mathf.Abs(motor[3]) > 1)
				stopper.GetComponent<Collider>().isTrigger = true;
            else
				stopper.GetComponent<Collider>().isTrigger = false;
            if (Mathf.Abs(motor[2]) > 1)
            {
				ramp.GetComponent<Rigidbody>().isKinematic = false;
                setMotor(ramp, -motor[2] / 40);
            }
            else
            {
				ramp.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        //stop robot if time is up
        if (timeUp)
        {
            frontRight.motorTorque = 0;
            frontLeft.motorTorque = 0;
            backRight.motorTorque = 0;
            backLeft.motorTorque = 0;
            arm1.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, 0);
            arm2.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, 0);
            intake.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, 0);
            ramp.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, 0);
            roof.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, 0);
        }
    }
}