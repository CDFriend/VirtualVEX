using UnityEngine;
using System.Collections;

public class Main_ShovelBot : vvRobotBase {
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider backLeft;
    public WheelCollider backRight;
    public GameObject armL1;
    public GameObject armL2;
    public GameObject armL3;
    public GameObject armR1;
    public GameObject armR2;
    public GameObject armR3;
    public GameObject anchor;
    public GameObject scoop;

    private vvComponentCollider r1Collider;
    private vvComponentCollider l1Collider;
    private vvComponentCollider scoopCollider;

    public float getLPower
    {
        get { return lMotorPowerPrev_; }
    }
    public float getRPower
    {
        get { return rMotorPowerPrev_; }
    }

    void Awake()
    {
        r1Collider = armR1.GetComponent<vvComponentCollider>();
        l1Collider = armL1.GetComponent<vvComponentCollider>();
        scoopCollider = scoop.GetComponent<vvComponentCollider>();
        robotID_ = "Scooper Bot";
    }

    void FixedUpdate()
    {
        bool timeUp = owner.getTimeLimit > 0 && owner.getTimeLeft <= 0 && tracker_.GetComponent<ModeTrackingScript>().disableOnTimeUp;
        //The robot should only move if the round hasn't ended
        //The only exeption is in solo untimed, which has no end.
        if (complete_ && !timeUp)
        {
            //Apply operator inputs to the proper mechanisms on the robot.
            if (scoopCollider.isColliding || r1Collider.isColliding || l1Collider.isColliding)
            {
                if ((lMotorPowerPrev_ > 0 && motor[1] > 0) || (lMotorPowerPrev_ < 0 && motor[1] < 0))
                    setMotors(frontLeft, backLeft, 0);
                else setMotors(frontLeft, backLeft, motor[1]);
                if ((rMotorPowerPrev_ > 0 && motor[0] > 0) || (rMotorPowerPrev_ < 0 && motor[0] < 0))
                    setMotors(frontRight, backRight, 0);
                else setMotors(frontRight, backRight, motor[0]);
            }
            else
            {
                setMotors(frontRight, backRight, motor[0]);
                setMotors(frontLeft, backLeft, motor[1]);
                lMotorPowerPrev_ = motor[1];
                rMotorPowerPrev_ = motor[0];
            }
            if (Mathf.Abs(motor[2]) > 5)
            {
				anchor.GetComponent<Rigidbody>().isKinematic = false;
                if (motor[2] > 0)
                {
                    setMotor(armL1, -80, motor[2] * 1.5f);
                    setMotor(armL2, -80, motor[2] * 1.5f);
                    setMotor(armL3, -80, motor[2] * 1.5f);
                    setMotor(armR1, -80, motor[2] * 1.5f);
                    setMotor(armR2, -80, motor[2] * 1.5f);
                    setMotor(armR3, -80, motor[2] * 1.5f);
                }
                else
                {
                    setMotor(armL1, 80, motor[2] * 1.5f);
                    setMotor(armL2, 80, motor[2] * 1.5f);
                    setMotor(armL3, 80, motor[2] * 1.5f);
                    setMotor(armR1, 80, motor[2] * 1.5f);
                    setMotor(armR2, 80, motor[2] * 1.5f);
                    setMotor(armR3, 80, motor[2] * 1.5f);
                }
            }
            else
				anchor.GetComponent<Rigidbody>().isKinematic = true;
            if (Mathf.Abs(motor[3]) < 5 && Mathf.Abs(motor[2]) < 5)
                scoop.GetComponent<Rigidbody>().isKinematic = true;
            else
                scoop.GetComponent<Rigidbody>().isKinematic = false;
            setMotor(scoop, motor[3]/10);
        }

        //stop robot if time is up
        if (timeUp)
        {
            frontRight.motorTorque = 0;
            frontLeft.motorTorque = 0;
            backRight.motorTorque = 0;
            backLeft.motorTorque = 0;
            armL1.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, 0);
            armL2.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, 0);
            armR1.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, 0);
            armR2.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, 0);
        }
    }
}
