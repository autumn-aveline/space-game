using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{

    [Range(-1f, 1f)]
    public float debugInputAccel;
    [Range(-1f, 1f)]
    public float debugInputTurn;
    [Range(-1f, 1f)]
    public float debugInputPitch;

    public float accelerationFactor;
    public float turnFactor;
    public float turnDampFactor;
    public float pitchFactor;
    public float pitchDampFactor;
    public float rollFactor;
    public float rollDampFactor;

    public float rollCorrectFactor;
    public float rollCorrectDampFactor;

    public float angularVelocityMax;
    public float angularVelocityMin;

    public float velocity;
    public float angularVelocity;
    public float acceleration;

    public GameObject objCamera;
    public GameObject objCameraTarget;

    private float autoLevelTimer;
    public float autoLevelWait;
    public float autoLevelSpeed;


/*     [Range(0,360)]
    public float worldRotation;
    public float forwardVelocity;
    //right is positive, left is negative
    public float lateralVelocity;
    public float altitudeVelocity;
    public Degree heading;
    //[Range(-360,360)]
    public Degree pitch;
    public float maxVelocity;
    private Vector3 globalVelocity; */

    private Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        //forwardVelocity = 0;
        //lateralVelocity = 0;
        //altitudeVelocity = 0;

        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float yaw = Input.GetAxisRaw("Joystick Z");
        float slider = Input.GetAxisRaw("Joystick Slider");

        float controlPitch = v;
        float controlTurn = h;
        float controlAccel = slider;
        float controlRoll = yaw;

        //var rotFactor = .25f;
/* 
        //transform.Rotate(0,h,0);
        //heading += h*rotFactor; // move yaw controls to WorldGen since they move the world
        pitch += -slider*rotFactor;
        //transform.eulerAngles = new Vector3(0,heading,0);
        transform.eulerAngles = new Vector3(pitch.GetValue(),0,0);
        forwardVelocity = maxVelocity*v;
        lateralVelocity = -maxVelocity*yaw; */


        Pitch(controlPitch);
        Turn(controlTurn);
        Roll(controlRoll);
        Accelerate(controlAccel);
        FixRoll(controlTurn, controlPitch);
        AutoLevel();
        UpdateCamera();

        UpdateInfo();
    }

    private void Pitch(float input) {
        // change the pitch, however you're gonna do it
        
        if (input != 0) {
            m_Rigidbody.AddRelativeTorque(Vector3.right * input * pitchFactor);
        }
    }

    private void Turn(float input) {
        // turn the ship

        // operate entirely on the Control Frame empty
        // (eventually. for now, before pitch and roll, gonna only manipulate the rigidbody)

        // if input is on, and angular velocity isnt at max, add force that direction
        // if input is off, but angular velocity is > 0, add force to counter that.
        //m_Rigidbody.AddForceAtPosition(Vector3.right * input * turnFactor,
        // TODO maybe write out the calculations for forceatpos on paper, and add that relatively?
        if (input != 0) {
            m_Rigidbody.AddRelativeTorque(Vector3.up * input * turnFactor);
        }
    }

    private void Roll(float input) {
        if (input != 0) {
            m_Rigidbody.AddRelativeTorque(Vector3.forward * -input * rollFactor);
        }
    }

    private void Accelerate(float input) {
        // accelerate
        // start with this one, first with just a constant value, then with input, then with smoothing it out.
        debugInputAccel = input;
        var force = Vector3.forward * accelerationFactor * debugInputAccel;
        m_Rigidbody.AddRelativeForce(force);
        
        acceleration = force.z;
    }


    private void FixRoll(float inputTurn, float inputPitch) {
        // somehow do autoroll one frame at at a time?
        var angularVelocity = m_Rigidbody.angularVelocity;
        
        if (inputTurn == 0  &&  inputPitch == 0) {
            if (angularVelocity.magnitude >= angularVelocityMin) {
                // apply a force in the opposite direction of angular velocity until it's done.
                var correctionForce = Vector3.zero - angularVelocity;
                correctionForce = correctionForce * pitchFactor / pitchDampFactor;
                m_Rigidbody.AddTorque(correctionForce);
            }
            else if (angularVelocity.magnitude!=0 && angularVelocity.magnitude<angularVelocityMin) {
                m_Rigidbody.angularVelocity = Vector3.zero;
            }
        }

        if (angularVelocity.magnitude > angularVelocityMax) {
            m_Rigidbody.angularVelocity = m_Rigidbody.angularVelocity.normalized * angularVelocityMax;
        }

        // TODO: 
        // if turn input is being held, then correct for forward velocity
        // if turn input is NOT being held, then ONLY correct for side velocity
        // also, use forces to counteract, instead of directly setting velocity. and tweak to feel.
        var velocityTargetMagnitude = m_Rigidbody.velocity.magnitude;
        var velocityLocal = transform.InverseTransformDirection(m_Rigidbody.velocity);
        velocityLocal.x = 0;
        velocityLocal.y = 0;
        velocityLocal.z = velocityTargetMagnitude;
        m_Rigidbody.velocity = transform.TransformDirection(velocityLocal);
    }

    private void AutoLevel() {
        // ONLY IF ANGULAR VELOCITY IS 0
        // if it's 0 and the autoLevelTimer less than autoLevelWait, add Time.deltaTime to the timer.

        // if angular velocity isn't 0, and autoLevelTimer is NOT 0, set autoLevelTimer to 0

        // if autoLevelTimer is >= autoLevelWait, then... auto-level. 

        // since we're checking angular velocity to see whether or not to auto-turn, 
        // we shouldn't use physics to turn it back, just adjust the rotation, i guess?

        // only add to the autoLevelTimer when the ship is not rotating;
        var angVel = m_Rigidbody.angularVelocity.magnitude;
        if (angVel == 0  &&  autoLevelTimer < autoLevelWait) {
            autoLevelTimer += Time.deltaTime;
        }
        else if (angVel != 0  &&  autoLevelTimer != 0) {
            autoLevelTimer = 0;
        }


        var horizonAngle = 0;

        if (autoLevelTimer >= autoLevelWait  &&  transform.rotation.z != horizonAngle) {
            var targetRotation = transform.rotation;
            targetRotation.z = horizonAngle;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1/autoLevelSpeed); 
        }

        // get ship rotation on z axis
        // compare to level (0)
        // if it's not level
    }

    private void UpdateCamera() {
        // start out by just setting the camera's coords directly to the empty, to get a feel for it
        // then, comment that out, and make the camera gradually slide to the empty. 
        // figure out how to tweak those vars

        // maybe make camera rigidbody kinematic, and use transform.position.moveTowards?

        

    }

    private void UpdateInfo() {
        velocity = m_Rigidbody.velocity.magnitude;
        angularVelocity = m_Rigidbody.angularVelocity.magnitude;
    }
}
