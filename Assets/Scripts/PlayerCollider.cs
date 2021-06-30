using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public WalkControls player;


    void OnControllerColliderHit(ControllerColliderHit hit) {
        print("huh");
    }


    void OnTriggerStay(Collider other)
    {
        player.velocity = -player.velocity;
        //player.isColliding = true;

        // // find collision point and normal. You may want to average over all contacts
        // var point = other.contacts[0].point;
        // var dir = -other.contacts[0].normal; // you need vector pointing TOWARDS the collision, not away from it
        // // step back a bit
        // point -= dir;
        // RaycastHit hitInfo;
        // // cast a ray twice as far as your step back. This seems to work in all
        // // situations, at least when speeds are not ridiculously big
        // var angle = 0f;
        // if(other.collider.Raycast( new Ray( point, dir ), out hitInfo, 2 ) )
        // {
        //     // this is the collider surface normal
        //     var normal = hitInfo.normal;
        //     // this is the collision angle
        //     // you might want to use .velocity instead of .forward here, but it 
        //     // looks like it's already changed due to bounce in OnCollisionEnter
        //     angle = Vector3.Angle( -transform.forward, normal );
        // }

        // print(angle);
    }

    void OnTriggerEnter(Collider other) {
        Vector3[] directions = {player.transform.forward,
                                player.transform.right,
                                -player.transform.forward,
                                -player.transform.right};
        
        var pos = player.transform.position;
        Vector3[] positions = {new Vector3(pos.x, 0, pos.z),
                               new Vector3(pos.x, pos.y, pos.z),
                               new Vector3(pos.x, 2, pos.z)};


        for (int i=0; i<directions.Length; i++) {
            for (int j=0; j<positions.Length; j++) {
                if(CheckRaycast(other, positions[j], directions[i])) {
                    break;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //print("trigger exit");
        //player.isColliding = false;
    }

    bool CheckRaycast(Collider collider, Vector3 position, Vector3 direction)
    {   
        RaycastHit hitInfo;
        bool isHit = collider.Raycast(new Ray(player.transform.position, direction), out hitInfo, 5f);

        //var deflect = hitInfo.normal*Vector3.Dot(player.velocity, hitInfo.normal);
        //deflect *= player.walkingSpeed*Time.deltaTime;
        //player.position -= deflect;
        
        print(hitInfo.distance);
        player.position -= direction*(hitInfo.distance);

        return isHit;
    }
}
