using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Entity
{
    public float radius;
    public float length;
    public float rotationRate;
    public float numSides;
    public float centerDeadzone;

    public float currentRotation;
    public bool shipInStation;

    public float speedLimitCenter;
    public float speedLimitSurface;

    // potentially have it's own coordinate system? it's own WorldController kinda thing?
    // probably simpler to just change ship coords automatically based on

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        canPlayerEnter = true;
    }

    // Update is called once per frame
    void Update()
    {
        //print(gameObject + " " + numChildren);
        RotateStation();

        if (playerInEntity) {
            UpdateMaxSpeed();
        }
    }

    private void UpdateMaxSpeed() {
        var ship = WorldController.ship;
        var d = DistanceFromCenterLineLocal(ship.position);
        //print(ship.velocity.magnitude/ship.maxSpeed);
        //var ratio = ship.velocity.magnitude/ship.maxSpeed;
        //var oldMax = ship.maxSpeed;
        ship.maxSpeed = ((radius-d)/radius)*(speedLimitCenter-speedLimitSurface) + speedLimitSurface;
        //print(ship.velocity*ratio + " " + ratio);
        //ship.velocity = ship.velocity*oldMax/ship.maxSpeed;

        // wait this ratio thing won't work at all, it needs to be handled on the ship side. 
        // where it's stored as like, percentOfMaxSpeed or whatever and acceleration or input directly affects it, not speed? sounds complicated lol

    }

    public bool Land()
    {
        //print("yay");
        var d = DistanceFromGroundLocal(WorldController.ship.position);
        var down = Down(WorldController.ship.position);
        var landDistance = 20f;

        //print(d);
        
        if (d <= landDistance) {
            var v = down*-d;
            WorldController.ship.position += v;
            WorldController.ship.landed = true;
            return true;
        }
        return false;
        //WorldController.ship.landed = true;
    }

    public override void TakeOff()
    {   
        
        //WorldController.ship.landed = false;
    }

    // TODO 
    // precise enough for rough distance to know whether to land or not, not precise enough for landing
    // WAIT NO IT IS, THE MODEL WAS OFF
    public float DistanceFromGroundLocal(Vector3 point) {
        var xyPos = new Vector2(point.x, point.y);
        var anglePoint = Vector2.Angle(Vector2.down, xyPos);
        var angleSegment = 360f/numSides;
        var angle = Mathf.Abs(angleSegment/2 - (anglePoint % angleSegment))*Mathf.Deg2Rad;
        var d = radius * Mathf.Sin(90*Mathf.Deg2Rad - angleSegment*Mathf.Deg2Rad/2) - DistanceFromCenterLineLocal(point)*Mathf.Cos(angle);
        return d;
    }

    public Vector3 Down(Vector3 point) {
        var xyPos = new Vector2(point.x, point.y);
        var anglePoint = Vector2.Angle(Vector2.down, xyPos);
        var angleSegment = 360f/numSides;
        var angle = Mathf.Abs(angleSegment/2 - (anglePoint % angleSegment))*Mathf.Deg2Rad;
        var section = Mathf.Floor(anglePoint/angleSegment);
        var angleSection = section*angleSegment*Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleSection), Mathf.Sin(angleSection), 0);
    }

    // Override
    public override bool IsInEntity(Vector3 position) {
        if (gameObject.name == "Station with buildings(Clone)") {
            int i = 2;
        }

        var zFront = this.position.z - (length / 2);
        var zBack = this.position.z + (length / 2);

        if (position.z < zFront  ||  position.z > zBack) { return false; }

        var shipXY = new Vector2(position.x, position.y);
        var stationXY = new Vector2(this.position.x, this.position.y);

        var d = Vector2.Distance(stationXY, shipXY);
        if (d <= radius) { return true; }
        else { return false; }
    }

    // Override
    public override bool IsInEntityLocal(Vector3 position) {
        var zFront = -(length / 2);
        var zBack = (length / 2);

        if (position.z < zFront  ||  position.z > zBack) { return false; }

        var shipXY = new Vector2(position.x, position.y);
        var stationXY = new Vector2(position.x, position.y);

        var d = Vector2.Distance(stationXY, shipXY);
        if (d <= radius) { return true; }
        else { return false; }
    }


    public bool IsInStation(Vector3 shipPosition) {
        var zFront = position.z - (length/2);
        var zBack = position.z + (length/2);

        if (shipPosition.z < zFront  ||  shipPosition.z > zBack) { return false; }

        var shipXY = new Vector2(shipPosition.x, shipPosition.y);
        var stationXY = new Vector2(position.x, position.y);

        var d = Vector2.Distance(stationXY, shipXY);
        if (d <= radius) { return true; }
        else { return false; }
    }

    public bool IsInStationLocal(Vector3 shipPosition) {
        var zFront = -(length / 2);
        var zBack  = (length / 2);

        if (shipPosition.z < zFront  ||  shipPosition.z > zBack) { return false; }

        var shipXY = new Vector2(shipPosition.x, shipPosition.y);
        var stationXY = new Vector2(position.x, position.y);

        var d = Vector2.Distance(stationXY, shipXY);
        if (d <= radius) { return true; }
        else { return false; }
    }

    public float DistanceFromCenterLine(Vector3 point) {
        var pointXY = new Vector2(point.x, point.y);
        var stationXY = new Vector2(position.x, position.y);
        return Vector2.Distance(stationXY, pointXY);
    }

    public float DistanceFromCenterLineLocal(Vector3 point) {
        var pointXY = new Vector2(point.x, point.y);
        var stationXY = Vector3.zero;
        return Vector2.Distance(stationXY, pointXY);
    }

    public override void EnterEntity()
    {
        var ship = WorldController.ship;
        playerInEntity = true;
        ship.localSpace = localSpace;
        ((StationSpace)localSpace).rotationOffset = currentRotation;
        ship.position = localSpace.ConvertToLocalCoordinates(ship.position);
        ship.rotation = localSpace.ConvertToLocalRotation(ship.rotation);
        gameObject.transform.parent = null;

        ship.currentLocation = this;

        //parent.gameObject.transform.parent = this.transform;
    }

    public override void ExitEntity()
    {
        var ship = WorldController.ship;
        playerInEntity = false;
        ship.localSpace = parent.localSpace;
        ship.position = localSpace.ConvertToParentCoordinates(ship.position);
        ship.rotation = localSpace.ConvertToParentRotation(ship.rotation);
        ((StationSpace)localSpace).rotationOffset = 0;
        gameObject.transform.parent = parent.gameObject.transform;

        ship.currentLocation = parent;

        // FOR TEST/PLAYING!! need to figure out architecture for this bit tho TODO
        ship.maxSpeed = 1000;

        //parent.gameObject.transform.parent = this.transform;
    }

    public void EnterStation(ShipControls ship) {
        shipInStation = true;
        //ship.localSpace = space;
        ship.localSpace = localSpace;
        //localSpace = space;
        ((StationSpace)localSpace).rotationOffset = currentRotation;
        ship.position = localSpace.ConvertToLocalCoordinates(ship.position);
        ship.rotation = localSpace.ConvertToLocalRotation(ship.rotation);
        gameObject.transform.parent = null;
    }

    public void ExitStation(ShipControls ship, ISpace worldSpace, Transform world) {
        shipInStation = false;
        ship.localSpace = worldSpace;
        //localSpace = worldSpace;
        ship.position = localSpace.ConvertToParentCoordinates(ship.position);
        ship.rotation = localSpace.ConvertToParentRotation(ship.rotation);
        ((StationSpace)localSpace).rotationOffset = 0;
        gameObject.transform.parent = world;
    }

    private void RotateStation() {
        Rotate(new Vector3(0, 0, rotationRate*Time.deltaTime));
        currentRotation += rotationRate*Time.deltaTime;

        if (playerInEntity && DistanceFromCenterLineLocal(WorldController.ship.position) < centerDeadzone) {
            WorldController.ship.Rotate(new Vector3(0, 0, -rotationRate*Time.deltaTime));
        }
    }
}
