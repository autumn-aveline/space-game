using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControls : PlayerControls
{
    public GameObject objWorld;
    public GameObject objSkybox;

    public bool landed;

    public Entity currentLocation;

    [Header("Acceleration")]
    [Range(0f, 4500)]
    public float maxSpeed;
    [Range(0f, 2f)]
    public float accelerationRate;
    
    [Header("Pitch")]
    [Range(0f, 2f)]
    public float pitchRate;
    
    [Header("Turn")]
    [Range(0f, 2f)]
    public float turnRate;

    [Header("Roll")]
    [Range(0f, 3f)]
    public float rollRate;

    [Header("Other")]
    [Range(0.0f,0.2f)]
    public float minimumAngularVelocity;
    [Range(1, 200)]
    public float maximumAngularVelocity;
    [Range(1, 250)]
    public float angularVelocityDamping;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //angularVelocity = new Vector3(0,0,10);
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float yaw = Input.GetAxisRaw("Joystick Z");
        float slider = Input.GetAxisRaw("Joystick Slider");
        float spacebar = Input.GetAxisRaw("Jump");

        float controlPitch = v;
        float controlTurn = h;
        float controlAccel = slider;
        float controlRoll = yaw;
        var controlLand = KeyCode.Space;

        if (Input.GetKeyDown(controlLand)) {
            if (landed == false) { Land(); }
            else { TakeOff(); }
        }

        Pitch(controlPitch);
        Turn(controlTurn);
        Roll(controlRoll);
        Accelerate(controlAccel);
        FixRoll(controlTurn, controlPitch, controlRoll);
        AutoLevel();

        UpdateInfo();

        if (velocity.magnitude > maxSpeed) {
            velocity = velocity.normalized * maxSpeed;
        }
        //print(parent.GetType());
        //if (parent.GetType() == typeof(Station)) {
            //print(((Station)currentLocation).Down(position));
        //}
    }

    private void Pitch(float input) {
        if (input == 0) { return; }
        AddTorque(new Vector3(input*pitchRate, 0,0));
    }

    private void Turn(float input) {
        if (input == 0) { return; }
        AddTorque(new Vector3(0, input*turnRate, 0));
    }

    private void Roll(float input) {
        if (input == 0) { return; }
        AddTorque(new Vector3(0,0, -input*rollRate));
    }

    private void Accelerate(float input) {
        if (input == 0) { return; }
        AddForce(new Vector3(0,0, input * accelerationRate));
    }

    private void Land() {
        //var b = currentLocation.Land();
        //currentLocation.Land();

        if (landed) {
            velocity = Vector3.zero;
            angularVelocity = Vector3.zero;
        }
        
    }

    private void TakeOff() {
        //currentLocation.TakeOff();
        landed = false;
    }


    // TODO
    // probably enforce max angular velocity for each axis independently, with independent damping factors
    private void FixRoll(float inputTurn, float inputPitch, float inputRoll) {
        if (inputTurn == 0  &&  inputPitch == 0  &&  inputRoll == 0) {
            if (angularVelocity.magnitude >= minimumAngularVelocity) {
                // apply a force in the opposite direction of angular velocity until it's done.
                var correctionForce = Vector3.zero - angularVelocity;
                correctionForce = correctionForce * angularVelocityDamping/1000;
                AddTorque(correctionForce);
            }
            else if (angularVelocity.magnitude != 0  &&  angularVelocity.magnitude < minimumAngularVelocity) {
                angularVelocity = Vector3.zero;
            }
        }

        if (angularVelocity.magnitude > maximumAngularVelocity) {
            angularVelocity = angularVelocity.normalized * maximumAngularVelocity;
        }

        // TODO: 
        // if turn input is being held, then correct for forward velocity
        // if turn input is NOT being held, then ONLY correct for side velocity
        // also, use forces to counteract, instead of directly setting velocity. and tweak to feel.
        var velocityTargetMagnitude = velocity.magnitude;
        if (Vector3.Dot(velocity, Vector3.forward) < 0) { velocityTargetMagnitude = -velocityTargetMagnitude; }
        var velocityLocal = transform.InverseTransformDirection(velocity);
        velocityLocal.x = 0;
        velocityLocal.y = 0;
        velocityLocal.z = velocityTargetMagnitude;
        velocity = transform.TransformDirection(velocityLocal);
    }

    private void AutoLevel() {

    }

    private void UpdateInfo() {

    }
}
