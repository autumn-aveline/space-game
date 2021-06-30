using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class WalkControls : PlayerControls
{

    public float walkingSpeed;
    public float runningSpeed;
    private float currentSpeed;

    //public Entity currentLocation;

    public Vector3 colliderNearestPoint;

    public bool isColliding;

    public Canvas menu;

    void Start()
    {
        colliderNearestPoint = Vector3.zero;
        velocity = Vector3.zero;
        isColliding = false;
        currentSpeed = walkingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        var controlRun = KeyCode.LeftShift;
        var controlPause = KeyCode.Return;

        if (Input.GetKeyDown(controlRun)) {
            currentSpeed = runningSpeed;
        } 
        else if (Input.GetKeyUp(controlRun)) {
            currentSpeed = walkingSpeed;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        //AddForce(move * walkingSpeed * Time.deltaTime);
        //print(velocity);
        velocity = move * currentSpeed * Time.deltaTime;
        
        if (Input.GetKeyDown(controlPause)) {
            Cursor.lockState = CursorLockMode.None;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            menu.gameObject.SetActive(true);
        }
    }
}
