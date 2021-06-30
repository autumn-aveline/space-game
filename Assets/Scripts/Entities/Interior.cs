using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interior : Entity
{

    // furniture objects are placed in this class, including walls
    //public GameObject props;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Vector3 position, GameObject gameObject, InteriorSpace localSpace) {
        this.position = position;
        this.go = gameObject;
        this.localSpace = localSpace;
    }
}
