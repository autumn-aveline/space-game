using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{   
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 rotationEuler;

    public Vector3 acceleration;
    public Vector3 velocity;
    private Vector3 lastVelocity;
    public Vector3 angularVelocity;
    public float mass;

    public bool canPlayerEnter;
    public bool playerInEntity;

    public ISpace localSpace;
    public GameObject go;
    public GameObject mesh;
    
    public Entity parent;
    public Entity[] children;
    public Entity[] interiors;
    public Entity[] props;

    public int numChildren;
    public int numInteriors;
    public int numProps;
    private int childIndex;

    public float width;
    public float depth;
    public float height;


    // Start is called before the first frame update
    public virtual void Start()
    {
        rotation = Quaternion.Euler(0,0,0);
        if (children == null) { children = new Entity[10]; }
    }

    void FixedUpdate()
    {
        ApplyPhysics();
        rotationEuler = rotation.eulerAngles; 
    }

    public void AddChild(Entity entity) {
        if (children == null) { children = new Entity[10]; }
        if (children.Length == numChildren) { 
            var len = children.Length + 1000;
            var arr = new Entity[len];
            for (int i=0; i<children.Length; i++) {
                arr[i] = children[i];
            }
            children = arr;
        }

        // have to track this to re-set it after SetParent, bc for some reason it changes the gd scale
        //var scale = entity.transform.localScale;

        entity.parent = this;
        entity.gameObject.transform.SetParent(this.gameObject.transform);
        //entity.transform.localScale = scale;
        entity.childIndex = numChildren;

        //entity.gameObject.transform.localScale = Vector3.one;

        children[numChildren] = entity;
        numChildren++;
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

    // Use Entity's default height/width/depth to calculate, with origin at center on the ground.
    public virtual bool isPointInInterior(Vector3 point) {
        return false;
    }

    public virtual void EnterEntity() {

    }

    public virtual void ExitEntity() {
        //may not need but nice to have the framework just in case
        // probably will be useful once I implement parent spaces, and can change to the parent.
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

    public void AddForce(Vector3 force) {
        // f = ma
        // a = f/m
        // a = dv/dt
        // dv = a*Time.deltaTime
        // change in velocity = force*Time.deltaTime/mass
        var deltaV = force.magnitude/mass;
        force = (force/force.magnitude) * deltaV;
        velocity += force;
    }

    public void AddTorque(Vector3 torque) {
        // t = r x F
        angularVelocity += torque;
    }

    private void ApplyPhysics() {
        lastVelocity = velocity;
        Translate(velocity*Time.deltaTime);
        Rotate(angularVelocity*Time.deltaTime);
        acceleration = (lastVelocity - velocity)*Time.deltaTime;
    }

    public virtual bool Land() { return false; }
    public virtual void TakeOff() {}

    public virtual bool IsInEntity(Vector3 position) {
        // only return true in inherited classes with their own implementation
        return false;
    }

    public virtual bool IsInEntityLocal(Vector3 position) {
        return false;
    }

    public void UpdateEntity() {
        //if (localSpace != typeof(WorldSpace)) { return; }
        go.transform.localPosition = position;
        go.transform.localRotation = rotation;
    }

    public void UpdateChildren() {
        //if (children == null) { return; }
        for (int i=0; i<numChildren; i++) {
            //print(gameObject + " " + numChildren);
            //print(gameObject + " - " + numChildren + " - " + children.Length + " - " + children[i]);
            children[i].UpdateEntity();
            children[i].UpdateChildren();
        }
    }

    public void UpdateParents() {
        if (parent == null) { return; }
        for (int i=0; i<parent.numChildren; i++) {
            if (i != childIndex) { parent.children[i].UpdateEntity(); }
        }
        parent.UpdateParents();
    }


    //public void AddProp(string prop, Vector3 position, Quaternion rotation) {
    //    var objProp = props.transform.Find(prop);
    //    var gameObject = MonoBehaviour.Instantiate(objProp, position, rotation, transform);
    //}
}
