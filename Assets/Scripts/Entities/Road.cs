using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Entity
{

    public Vector3 start;
    public Vector3 end;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        canPlayerEnter = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
