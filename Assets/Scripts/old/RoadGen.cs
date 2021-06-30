using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGen : MonoBehaviour
{
    public GameObject spaceRing;
    public float ringDistance;
    public GameObject pos;
    public GameObject rot;
    public GameObject parent;
    private float global_bankAmount;

    // Start is called before the first frame update
    void Start()
    {
        pos.transform.position = new Vector3(0,0,0);
        global_bankAmount = 0f;

        GenerateStraightRoad(5000);
        //GenerateTurnDown(1590*3, 0);

        //GenerateBankRight(10*2, 30*3);
        //GenerateTurnRight(50*2, 90);
        //GenerateBankLeft(10*3, 30*3);
        //GenerateStraightRoad(50);
        //GenerateBankRight(120, 360);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaceNextRing() {
        GameObject obj;
        obj = Instantiate(spaceRing, pos.transform.position, pos.transform.rotation, parent.transform);
        obj.transform.Rotate(0,0,global_bankAmount);
    }

    void GenerateStraightRoad(int numRings) {
        //float z = 0;
        for (int i = 0; i < numRings; i++) {
            PlaceNextRing();
            //move coords by ringDistance amount
            pos.transform.Translate(0,0,ringDistance);
        }
    }

    void GenerateBank(int numRings, float tiltAmount) {
        float bankStep = tiltAmount / (numRings - 1);
        
        for (int i = 0; i < numRings; i++) {
            //place a ring
            PlaceNextRing();
            //move coords by RingDistance amount and rotate
            pos.transform.Translate(0,0,ringDistance);
            global_bankAmount = global_bankAmount + bankStep;
            //rot.transform.Rotate(0,0,bankStep);
        }
    }

    void GenerateBankRight(int numRings, float tiltAmount) {
        GenerateBank(numRings, -tiltAmount);
    }

    void GenerateBankLeft(int numRings, float tiltAmount) {
        GenerateBank(numRings, tiltAmount);
    }

    void GenerateTurn(int numRings, float turnAmount) { //, float tiltAmount) {
        float turnStep = turnAmount / (numRings - 1);

        for (int i = 0; i < numRings; i++) {
            //place a ring
            PlaceNextRing();
            //move coords and rotate for turn
            pos.transform.Translate(0,0,ringDistance);
            pos.transform.Rotate(0,turnStep,0);
        }
    }

    void GenerateTurnRight(int numRings, float turnAmount) {
        GenerateTurn(numRings, turnAmount);
    }

    void GenerateTurnLeft(int numRings, float turnAmount) {
        GenerateTurn(numRings, -turnAmount);
    }

    void GenerateTurnDown(int numRings, float turnAmount) {
        float turnStep = turnAmount / (numRings - 1);

        for (int i = 0; i < numRings; i++) {
            //place a ring
            PlaceNextRing();
            //move coords and rotate for turn
            pos.transform.Translate(0,0,ringDistance);
            pos.transform.Rotate(turnStep,0,0);
        }
    }
}
