using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorSpace : ISpace
{
    public Interior interior;

    public InteriorSpace(Interior interior) {
        this.interior = interior;
    }

    public void UpdateWorld() {
        var skybox = WorldController.skybox;
        interior.gameObject.transform.position = -InteriorController.player.position;
        interior.gameObject.transform.rotation = Quaternion.identity;
        //skybox.transform.rotation = Quaternion.identity;
    
        var angle = 0f;
        var axis = Vector3.zero;
        InteriorController.player.rotation.ToAngleAxis(out angle, out axis);
        interior.gameObject.transform.RotateAround(Vector3.zero, axis, -angle);
        //skybox.transform.RotateAround(Vector3.zero, axis, -angle);

        //interior.UpdateChildren();
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
