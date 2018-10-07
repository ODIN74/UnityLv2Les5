using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveLoadLevel {

    [MenuItem("Window/Experimental/Save Level")]
    public static void Save()
    {
        var path = EditorUtility.SaveFilePanel("Save Level", Application.dataPath, "LevelData", "xml");

        GameObject[] levelObjects = GameObject.FindObjectsOfType<GameObject>();
        List<SerializableObject> objectsList = new List<SerializableObject>();
        foreach (var obj in levelObjects)
        {
            objectsList.Add(new SerializableObject()
            {
                Name = obj.name,
                Position = obj.transform.position,
                Rotation = obj.transform.rotation,
                Scale = obj.transform.localScale,
                ObjectRigidbody = obj.GetComponent<Rigidbody>()
            });
        }
        XMLSerializator.Save(objectsList.ToArray(),path);

    }


    [MenuItem("Window/Experimental/Load Level")]
    public static void Load()
    {
        var path = EditorUtility.OpenFilePanel("Load Level", Application.dataPath, "xml");

        var objs = XMLSerializator.Load(path);

        foreach(var obj in objs)
        {
            GameObject prefab = Resources.Load<GameObject>(obj.Name);

            var tempObj = GameObject.Instantiate(prefab, obj.Position, obj.Rotation);
            tempObj.transform.localScale = obj.Scale;
            tempObj.name = obj.Name;
            if(tempObj.GetComponent<Rigidbody>() == null)
            {
                tempObj.AddComponent<Rigidbody>();
            }
            var tempObjRb = tempObj.GetComponent<Rigidbody>();
            tempObjRb.mass = obj.ObjectRigidbody.mass;
            tempObjRb.drag = obj.ObjectRigidbody.drag;
            tempObjRb.angularDrag = obj.ObjectRigidbody.angularDrag;
            tempObjRb.interpolation = obj.ObjectRigidbody.interpolation;
            tempObjRb.collisionDetectionMode = obj.ObjectRigidbody.collisionDetectionMode;
            tempObjRb.isKinematic = obj.ObjectRigidbody.isKinematic;
            tempObjRb.useGravity = obj.ObjectRigidbody.useGravity;
        }
    }

}
