using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static GameObject skybox;
    public static PlayerControls player;
    public static Entity currentEntity;
    //public static Entity parentEntity;
    //public static Entity childEntity;
    public static GameObject propsInterior;
    public static GameObject propsExterior;

    // dragged and dropped in Unity Editor
    public GameObject objSkybox;
    public PlayerControls objPlayer;
    public GameObject objCurrentEntity;
    public GameObject objParentEntity;
    //public GameObject objChildEntity;

    public GameObject objPropsInterior;
    public GameObject objPropsExterior;

    public Sector[] sector;
    public GameObject objSector;



    // PREFABS

    public GameObject spaceRing;



    // Roads, Stations, and other ships will all be arrays here

    // Start is called before the first frame update
    void Start()
    {
        skybox = objSkybox;
        player = objPlayer;
        propsInterior = objPropsInterior;
        propsExterior = objPropsExterior;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }


    private void UpdateWorld() {
        
    }
}
