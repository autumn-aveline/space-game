using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpace
{
    void UpdateWorld();
    Vector3 ConvertToLocalCoordinates(Vector3 parentPos);
    Vector3 ConvertToParentCoordinates(Vector3 localPos);
    Quaternion ConvertToLocalRotation(Quaternion parentRot);
    Quaternion ConvertToParentRotation(Quaternion localRot);
}
