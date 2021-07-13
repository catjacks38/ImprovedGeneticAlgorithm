using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NN;
using static NN.ActivationFunction;
using Model;

public class AgentNN : MonoBehaviour
{
    public AgentModel model = new AgentModel();

    public HingeJoint2D legRU;
    public HingeJoint2D legLU;

    public HingeJoint2D legRD;
    public HingeJoint2D legLD;

    public JointMotor2D legRUMotor;
    public JointMotor2D legLUMotor;
    
    public JointMotor2D legRDMotor;
    public JointMotor2D legLDMotor;

    public SpriteRenderer[] bodyPartSprites = new SpriteRenderer[5];

    public Transform body;
    public Rigidbody2D bodyRigidBody;

    public List<float> rpmPerLeg = new List<float>();

    private float maxSpeed = 150.0f;
    private float maxTorque = 100.0f;

    private void Start()
    {
        legRUMotor.maxMotorTorque = maxTorque;
        legLUMotor.maxMotorTorque = maxTorque;
        
        legRDMotor.maxMotorTorque = maxTorque;
        legLDMotor.maxMotorTorque = maxTorque;
    }

    private void Update()
    {
        // Checks to see if it is the fittest
        // Disables sprite rendering if not
        if (gameObject.tag == "fittest")
        {
            for (int i = 0; i < bodyPartSprites.Length; i++)
            {
                bodyPartSprites[i].enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < bodyPartSprites.Length; i++)
            {
                bodyPartSprites[i].enabled = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // Gets motor speeds from NN
        rpmPerLeg = model.forward(new List<float>() { bodyRigidBody.angularVelocity, body.eulerAngles.z, legRU.gameObject.transform.rotation.z, legLU.gameObject.transform.rotation.z, legRD.gameObject.transform.rotation.z, legLD.gameObject.transform.rotation.z});

        // Applies motor speeds to legs
        legRUMotor.motorSpeed = Mathf.Clamp(rpmPerLeg[0], -maxSpeed, maxSpeed);
        legLUMotor.motorSpeed = Mathf.Clamp(rpmPerLeg[1], -maxSpeed, maxSpeed);

        legRDMotor.motorSpeed = Mathf.Clamp(rpmPerLeg[2], -maxSpeed, maxSpeed);
        legLDMotor.motorSpeed = Mathf.Clamp(rpmPerLeg[3], -maxSpeed, maxSpeed);

        legRU.motor = legRUMotor;
        legLU.motor = legLUMotor;
        
        legRD.motor = legRDMotor;
        legLD.motor = legLDMotor;
    }
}
