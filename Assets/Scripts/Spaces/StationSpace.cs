using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationSpace : ISpace
{
    public GameObject objWorld;
    public GameObject objSkybox;
    public ShipControls ship;
    public Station station;

    public float rotationOffset;

    public StationSpace(Station station) {
        objWorld = WorldController.objWorld;
        objSkybox = WorldController.skybox;
        ship = WorldController.ship;
        this.station = station;
    }

    public void UpdateWorld() {
        // Detach the station from the World object in Unity. The station is going to be independent right now.
        // for the station, use the same rules as the world does, in WorldSpace
        // for the world... do the RotateAround thing, I guess? If I can get the first part working, I can get the second, I think
        //station.position = -objShip.position;
        station.gameObject.transform.localPosition = -ship.position;
        station.gameObject.transform.rotation = Quaternion.identity;//= Quaternion.Euler(0, 0, rotationOffset);
        //objWorld.transform.rotation = Quaternion.identity;
        objSkybox.transform.rotation = Quaternion.identity;

        float angle = 0;
        Vector3 axis = Vector3.zero;
        ship.rotation.ToAngleAxis(out angle, out axis);
        station.gameObject.transform.RotateAround(Vector3.zero, axis, -angle);
        //objWorld.transform.RotateAround(Vector3.zero, axis, -angle);
        objSkybox.transform.RotateAround(Vector3.zero, axis, -angle);

        //station.rotation = objStation.transform.rotation;

        //objStation.transform.position = Vector3.zero;
        //station.transform.Rotate(0, 0, -station.currentRotation + rotationOffset);

        float angle2;
        Vector3 axis2;
        station.rotation.ToAngleAxis(out angle2, out axis2);
        station.parent.transform.localPosition = -ship.position - station.position;
        station.parent.transform.rotation = Quaternion.identity;
        //station.parent.transform.rotation = ConvertToLocalRotation(station.parent.transform.rotation);
        //station.parent.transform.RotateAround(-station.position, station.transform.forward, -station.currentRotation + rotationOffset);
        objSkybox.transform.RotateAround(Vector3.zero, station.transform.forward, -station.currentRotation);
        station.parent.transform.RotateAround(Vector3.zero, axis, -angle);
        station.parent.transform.RotateAround(station.transform.position, station.transform.forward, -station.currentRotation);
        //station.parent.transform.Rotate(0, 0, -station.currentRotation + rotationOffset);
        //objSkybox.transform.Rotate(0, 0, -station.currentRotation + rotationOffset);

        station.UpdateParents();

    }

    public void UpdateWorld_bak() {

        objWorld.transform.position = -ConvertToParentCoordinates(ship.position);
        objWorld.transform.rotation = Quaternion.identity;//ConvertToParentRotation(objShip.rotation);
        objSkybox.transform.rotation = Quaternion.identity;//ConvertToParentRotation(objShip.rotation);
        
        float angle = 0;
        Vector3 axis = Vector3.zero;
        ship.rotation.ToAngleAxis(out angle, out axis);
        objWorld.transform.RotateAround(Vector3.zero, axis, -angle);
        objSkybox.transform.RotateAround(Vector3.zero, axis, -angle);

        objWorld.transform.Rotate(0, 0, -station.currentRotation + rotationOffset);
        objSkybox.transform.Rotate(0, 0, -station.currentRotation + rotationOffset);
    }





    public Vector3 ConvertToLocalCoordinates(Vector3 parentPos) {
        var origin = station.position;
        var r = station.DistanceFromCenterLine(parentPos);
        var local = parentPos - origin;
        // rotate local position by station.currentRotation degrees, around the origin?
        return Quaternion.Euler(0, 0, -rotationOffset) * local;
    }

    public Vector3 ConvertToParentCoordinates(Vector3 localPos) {
        var origin = station.position;
        var r = station.DistanceFromCenterLineLocal(localPos);
        var global = localPos + origin;
        return Quaternion.Euler(0, 0, rotationOffset) * global;
    }

    public Quaternion ConvertToLocalRotation(Quaternion parentRot) {
        var r = Quaternion.Euler(0, 0, station.currentRotation);
        r = Quaternion.Inverse(r)*parentRot;
        return r;
    }

    public Quaternion ConvertToParentRotation(Quaternion localRot) {
        // get radius from center
        // no, get offset and... currentrotation? why not just try current?
        var r = Quaternion.Euler(0, 0, station.currentRotation);
        r = r*localRot;
        return r;
    }
}