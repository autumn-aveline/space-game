using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEntity : Entity
{
    public Vector3 acceleration;
    public Vector3 velocity;
    private Vector3 lastVelocity;
    public Vector3 angularVelocity;
    public float mass;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyPhysics();
    }

    public void AddForce(Vector3 force) {
        // f = ma
        // a = f/m
        // a = dv/dt
        // dv = a*Time.deltaTime
        // change in velocity = force*Time.deltaTime/mass
        var deltaV = force.magnitude/mass;
        force = (force/force.magnitude) * deltaV;
        velocity += force;
    }

    public void AddTorque(Vector3 torque) {
        // t = r x F
        angularVelocity += torque;
    }

    private void ApplyPhysics() {
        lastVelocity = velocity;
        Translate(velocity*Time.deltaTime);
        Rotate(angularVelocity*Time.deltaTime);
        acceleration = (lastVelocity - velocity)*Time.deltaTime;
    }

}
