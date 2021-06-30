using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Entity
{
    public Vector2 frontDoorPos;
    public Vector2 facadeDimensions;
    
    public float depth;
    public float height;
    public int numFloors;
    public Transform transform;

    public Interior interior;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        canPlayerEnter = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // center point should be in the center of the facade, on the ground
    public override bool isPointInInterior(Vector3 point)
    {
        // subtract the position of the building from the point, 
        // bc it's easier to compare to a normalized point
        point = point - position;

        point = transform.InverseTransformPoint(point);
        print(point);

        var z = facadeDimensions.x/2;

        if (point.x>0 | point.x<-depth) { return false; }
        if (point.z<-z || point.z>z) { return false; }

        return true;
    }
}
