using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpace : ISpace
{
    public GameObject objWorld;
    public GameObject objSkybox;
    public ShipControls objShip;

    public WorldSpace(GameObject world, GameObject skybox, ShipControls ship) {
        objWorld = world;
        objSkybox = skybox;
        objShip = ship;
    }

    public void UpdateWorld() {
        objWorld.transform.position = -objShip.position;
        objWorld.transform.rotation = Quaternion.identity;
        objSkybox.transform.rotation = Quaternion.identity;
        
        float angle = 0;
        Vector3 axis = Vector3.zero;
        objShip.rotation.ToAngleAxis(out angle, out axis);
        objWorld.transform.RotateAround(Vector3.zero, axis, -angle);
        objSkybox.transform.RotateAround(Vector3.zero, axis, -angle);
    }

    public Vector3 ConvertToLocalCoordinates(Vector3 parentPos) {
        return parentPos;
    }

    public Vector3 ConvertToParentCoordinates(Vector3 localPos) {
        return localPos;
        // TODO eventually, this top level space will have a "parent" that returns latlong coordinates
    }

    public Quaternion ConvertToLocalRotation(Quaternion parentRot) {
        return parentRot;
    }

    public Quaternion ConvertToParentRotation(Quaternion localRot) {
        return localRot;
    }
}
