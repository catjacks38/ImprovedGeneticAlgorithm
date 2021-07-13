using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public HingeJoint2D legR;
    public HingeJoint2D legL;

    public float Speed = 25.0f;
    public float MaxTorque = 1000.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            JointMotor2D motor = new JointMotor2D();

            motor.motorSpeed = Speed;
            motor.maxMotorTorque = MaxTorque;

            legL.motor = motor;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            JointMotor2D motor = new JointMotor2D();

            motor.motorSpeed = -Speed;
            motor.maxMotorTorque = MaxTorque;

            legL.motor = motor;
        }

        else if (Input.GetKey(KeyCode.Semicolon))
        {
            JointMotor2D motor = new JointMotor2D();

            motor.motorSpeed = Speed;
            motor.maxMotorTorque = MaxTorque;

            legR.motor = motor;
        }
        else if (Input.GetKey(KeyCode.Quote))
        {
            JointMotor2D motor = new JointMotor2D();

            motor.motorSpeed = -Speed;
            motor.maxMotorTorque = MaxTorque;

            legR.motor = motor;
        }

        else
        {
            JointMotor2D motor = new JointMotor2D();

            motor.motorSpeed = 0.0f;
            motor.maxMotorTorque = 0.0f;

            legR.motor = motor;
            legL.motor = motor;
        }

    }
}
