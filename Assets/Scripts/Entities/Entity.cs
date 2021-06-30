using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{   
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 rotationEuler;

    public bool canPlayerEnter;
    public bool playerInEntity;

    public ISpace localSpace;
    public GameObject go;
    public GameObject mesh;
    
    public Entity parent;
    public Entity[] interiors;
    public Entity[] props;

    public int numInteriors;
    public int numProps;

    public float width;
    public float depth;
    public float height;


    // Start is called before the first frame update
    public virtual void Start()
    {
        rotation = Quaternion.Euler(0,0,0);
        //if (children == null) { children = new Entity[10]; }
    }

    void FixedUpdate()
    {
        rotationEuler = rotation.eulerAngles; 
    }

    public void AddInterior(Entity entity) {
        if (interiors == null) { interiors = new Entity[10]; }
        if (interiors.Length == numInteriors) { 
            var len = interiors.Length + 1000;
            var arr = new Entity[len];
            for (int i=0; i<interiors.Length; i++) {
                arr[i] = interiors[i];
            }
            interiors = arr;
        }

        entity.parent = this;
        entity.gameObject.transform.SetParent(this.gameObject.transform);

        interiors[numInteriors] = entity;
        numInteriors++;
    }

    public void AddProp(string prop, Vector3 position, Quaternion rotation, string propPack = "interior") {
        Transform objProp;
        if (propPack == "interior") { objProp = WorldController.propsInterior.transform.Find(prop); }
        else if (propPack == "exterior") { objProp = WorldController.propsExterior.transform.Find(prop); }
        else { return; }
        var gameObject = MonoBehaviour.Instantiate(objProp, position, rotation, transform);
        
        gameObject.localPosition = position;
        gameObject.localRotation = rotation;
    }

    // Use Entity's default height/width/depth to calculate, with origin at center on the ground.
    public virtual bool isPointInInterior(Vector3 point) {
        return false;
    }


    public void Translate(Vector3 v) {
        v = rotation * v;
        position += v;
        //if (v == null) { return; }
        //position = localSpace.Translate(position, v);
    }

    public void Rotate(Vector3 v) {
        rotation = rotation * Quaternion.Euler(v);
        //rotation = localSpace.Rotate(rotation, v);
    }

    public void UpdateEntity() {
        go.transform.localPosition = position;
        go.transform.localRotation = rotation;
    }


}
