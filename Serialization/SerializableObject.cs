using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableObject
{
    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public SerializableRigidbody ObjectRigidbody;

    public struct SerializableRigidbody
    {
        public float mass;
        public float drag;
        public float angularDrag;
        public bool useGravity;
        public bool isKinematic;
        public RigidbodyInterpolation interpolation;
        public CollisionDetectionMode collisionDetectionMode;

        public static implicit operator Rigidbody(SerializableRigidbody value)
        {
           var tempBody =  new Rigidbody();
            tempBody.mass = value.mass;
            tempBody.drag = value.drag;
            tempBody.angularDrag = value.angularDrag;
            tempBody.useGravity = value.useGravity;
            tempBody.isKinematic = value.isKinematic;
            tempBody.interpolation = value.interpolation;
            tempBody.collisionDetectionMode = value.collisionDetectionMode;
            return tempBody;
        }

        public static implicit operator SerializableRigidbody(Rigidbody value)
        {
            var tempBody = new SerializableRigidbody();
            tempBody.mass = value.mass;
            tempBody.drag = value.drag;
            tempBody.angularDrag = value.angularDrag;
            tempBody.useGravity = value.useGravity;
            tempBody.isKinematic = value.isKinematic;
            tempBody.interpolation = value.interpolation;
            tempBody.collisionDetectionMode = value.collisionDetectionMode;
            return tempBody;
        }
    }
}
