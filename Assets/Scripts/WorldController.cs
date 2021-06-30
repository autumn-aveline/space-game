using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static GameObject objWorld;
    public static GameObject skybox;
    public static ShipControls ship;

    public GameObject objSkybox;
    public ShipControls objShip;

    public Sector sector;
    public GameObject objSector;

    public ISpace localSpace;

    // PREFABS

    public GameObject spaceRing;

    // STATION TESTING
    
    public GameObject objStationMesh1;
    public GameObject objStationMesh2;
    private GameObject objStation1;
    private GameObject objStation2;

    public float station1RotationRate;
    public Vector3 station1Pos;
    public float station1Radius;
    public float station1Length;

    public float station2RotationRate;
    public Vector3 station2Pos;

    public static Sector sectorGlobal;



    // Roads, Stations, and other ships will all be arrays here

    // Start is called before the first frame update
    void Start()
    {
        sectorGlobal = sector;
        skybox = objSkybox;
        ship = objShip;
        InitializeSector();
        InitializeStations();
        InitializeRoads();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //StationStuff();
        //if (sector.children[0] != null) {
        //print(sector.children[0].IsInEntity(ship.position)); }
        CheckChildren();
        UpdateWorld();
    }


    private void CheckChildren() {
        int i;
        Entity entity;
        for (i=0; i<sector.numChildren; i++) {
            entity = sector.children[i];
            if (entity.IsInEntity(ship.position) && entity.playerInEntity==false) {
                entity.EnterEntity();
                localSpace = entity.localSpace;
                break;
            }
            else if (!entity.IsInEntityLocal(ship.position) && entity.playerInEntity==true) {
                entity.ExitEntity();
                localSpace = entity.parent.localSpace;
            }
        }
    }


    private void InitializeRoads() {
        var numRings = 26+10;
        var ringDistance = 30;
        var startPos = new Vector3(0, 0, -513);
        var road = EntityBuilder.GenerateStraightRoad(numRings, spaceRing, ringDistance, startPos, Quaternion.identity);
        sector.AddChild(road);

        var road2 = EntityBuilder.GenerateStraightRoad(30, spaceRing, ringDistance, new Vector3(0, 0, 0), Quaternion.identity);
        var station1 = sector.children[0];
        road2.position.z = -.9f; // THIS IS A BANDAID, TODO FIX
        road2.rotation *= Quaternion.Euler(0, 0, 0);
        station1.AddChild(road2);

        var road3 = EntityBuilder.GenerateStraightRoad(10000, spaceRing, ringDistance*10f, new Vector3(0, 0, 1477), Quaternion.identity, 3);
        sector.AddChild(road3);
    }


    private void InitializeSector() {
        sector.position = Vector3.zero;
        sector.go = objSector;
        sector.localSpace = new SectorSpace(sector);
        ship.localSpace = sector.localSpace;
        localSpace = sector.localSpace;
        
        ship.currentLocation = sector;
    }



    /*************************************************************
    /          INITIALIZE STATIONS
    /*************************************************************/

    private void InitializeStations() {
        var position1 = new Vector3(0, 0, 1000);
        var rotation1 = Quaternion.identity;

        var position2 = new Vector3(0, 0, 110000);
        var rotation2 = Quaternion.identity; 

        var station1 = EntityBuilder.InitializeStation(objStationMesh1, station1Radius, station1Length, station1RotationRate, 12, 20, position1, rotation1);
        var station2 = EntityBuilder.InitializeStation(objStationMesh2, 800, 8000, station2RotationRate, 32, 10, station2Pos, rotation2);

        station1.speedLimitCenter = 150;
        station1.speedLimitSurface = 14;
        station2.speedLimitCenter = 150;
        station2.speedLimitSurface = 600;

        sector.AddChild(station1);
        sector.AddChild(station2);
    }



    private void StationStuff() {
        var station1 = objStation1.GetComponent<Station>();
        //var ship = objShip.GetComponent<ShipControls>();
        var rotationDelta = new Vector3(0, 0, station1.rotationRate*Time.deltaTime);
        rotationDelta = ship.rotation * rotationDelta;

        if (station1.shipInStation == false) {
            if (station1.IsInStation(ship.position) == true) {
                station1.EnterStation(ship);
            }
        }
        else if (station1.shipInStation == true) {
            if (station1.IsInStationLocal(ship.position) == false) {
                //station1.ExitStation(ship, worldSpace, objStations.transform);
            }
        }
    }

    private void UpdateWorld() {
        //print(objShip.localSpace);
        //print(localSpace);
        localSpace.UpdateWorld();
        //sector.UpdateEntity();
        //sector.gameObject.transform.Translate(0, 0, -5);
    }
}
