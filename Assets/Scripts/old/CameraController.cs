using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject objCameraTarget;
    public GameObject objCameraLookahead;
    public ShipControls objShip;

    [Range(0f, 5f)]
    public float followHeight;
    [Range(5f, 30f)]
    public float followDistance;
    public float followDistanceStretch;
    public float followDamp;
    public float followBrakeDamp;

    public float turnDamp;
    public float pitchDamp;
    [Range(1f, 25f)]
    public float rollDamp;

    public float lookaheadDistance;

    private Rigidbody rigidbody;
    public float smoothTime;

    

    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateCamera();
    }

    private void InitializeVariables() {
        // set lookahead distance and follow behind distance here, eventually
        rigidbody = GetComponent<Rigidbody>();
    }

    private void UpdateCamera() {
        transform.position = objCameraTarget.transform.position;
        transform.LookAt(objCameraLookahead.transform, objCameraLookahead.transform.up);

        objCameraLookahead.transform.localPosition = new Vector3(0,0,lookaheadDistance);

        var maxAcceleration = 300;
        var newCameraZ = 0.0f;

        var acceleration = objShip.acceleration.magnitude;

        if (acceleration < 0) {
            newCameraZ = -acceleration*followDistanceStretch/(followBrakeDamp*maxAcceleration) - followDistance;
        }
        else {
            newCameraZ = -acceleration*followDistanceStretch/maxAcceleration - followDistance;
        }

        var localAngularVelocity = objCameraTarget.transform.InverseTransformVector(objShip.angularVelocity);
        var turnAmount = localAngularVelocity.y;
        var pitchAmount = localAngularVelocity.x;
        var rollAmount = localAngularVelocity.z;

        var newCameraX = turnAmount/turnDamp;
        var newCameraY = -pitchAmount/pitchDamp + followHeight;
        var newCameraRoll = -rollAmount/rollDamp;

        var newCameraPos = new Vector3(newCameraX, newCameraY, newCameraZ);
        //newCameraPos = Quaternion.AngleAxis(turnAmount*Mathf.Rad2Deg, objCameraTarget.transform.up) * newCameraPos;
        objCameraTarget.transform.localPosition = Vector3.Lerp(objCameraTarget.transform.localPosition, newCameraPos, followDamp*Time.deltaTime);
    

        // WAIT NO
        // I have to SET the localRotation in the OPPOSITE direction of the roll, and then lerp it

        var newCameraUp = new Vector3(0, 0, newCameraRoll);
        newCameraUp = objCameraTarget.transform.InverseTransformVector(newCameraUp);

        transform.Rotate(newCameraUp, Space.Self);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, objCameraTarget.transform.localRotation, rollDamp*Time.deltaTime);
        transform.LookAt(objCameraLookahead.transform, transform.up);
    }
}
