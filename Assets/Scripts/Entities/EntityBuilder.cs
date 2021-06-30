using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBuilder
{
    public static Station InitializeStation(GameObject mesh,
                                            float radius,
                                            float length,
                                            float rotationRate,
                                            float numSides,
                                            float centerDeadzone,
                                            Vector3 position,
                                            Quaternion rotation) {

        var gameObject = MonoBehaviour.Instantiate(mesh, position, rotation);
        gameObject.transform.localScale = new Vector3(radius, radius, length/2);
        
        gameObject.AddComponent<Station>();
        var station = gameObject.GetComponent<Station>();

        station.radius = radius;
        station.length = length;
        station.rotationRate = rotationRate;
        station.numSides = numSides;
        station.centerDeadzone = centerDeadzone;
        station.position = position;
        station.rotation = rotation;
        station.go = gameObject;
        station.mesh = mesh;
        station.children = new Entity[10];
        station.localSpace = new StationSpace(station);

        return station;
    }

    public static Road GenerateStraightRoad(int numRings, 
                                            GameObject ringMesh,
                                            float ringDistance,
                                            Vector3 startPos, 
                                            Quaternion rotation,
                                            float ringScale = 1f) {
        
        var gameObject = new GameObject("Straight road");
        gameObject.transform.position = Vector3.zero;
        gameObject.AddComponent<Road>();
        
        var road = gameObject.GetComponent<Road>();
        road.start = startPos;
        road.go = gameObject;
        road.mesh = ringMesh;

        // Generate road rings

        var roadPos = new GameObject("roadPos");
        var roadRot = new GameObject("roadRot");
        roadPos.transform.parent = gameObject.transform;
        roadRot.transform.parent = gameObject.transform;

        GameObject obj;

        for (int i=0; i<numRings; i++) {
            obj = MonoBehaviour.Instantiate(ringMesh, roadPos.transform.position, roadRot.transform.rotation, gameObject.transform);
            roadPos.transform.Translate(0, 0, ringDistance);
            obj.transform.localScale *= ringScale;
        }
        road.end = roadPos.transform.position;
        road.position = road.start;

        return road;
    }




    //public static Interior GetApartmentFloor() {

    //}

}
