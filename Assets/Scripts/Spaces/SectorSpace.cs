using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorSpace : ISpace
{
    public Sector sector;

    public SectorSpace(Sector sector) {
        this.sector = sector;
    }

    public void UpdateWorld() {
        var skybox = WorldController.skybox;
        sector.gameObject.transform.position = -WorldController.ship.position;
        sector.gameObject.transform.rotation = Quaternion.identity;
        skybox.transform.rotation = Quaternion.identity;
    
        var angle = 0f;
        var axis = Vector3.zero;
        WorldController.ship.rotation.ToAngleAxis(out angle, out axis);
        sector.gameObject.transform.RotateAround(Vector3.zero, axis, -angle);
        skybox.transform.RotateAround(Vector3.zero, axis, -angle);

        sector.UpdateChildren();
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
