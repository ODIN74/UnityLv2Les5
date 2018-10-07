using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public static class XMLSerializator
{
    private static XmlSerializer _serializer;

    static XMLSerializator()
    {
        _serializer = new XmlSerializer(typeof(SerializableObject[]));
    }

    public static void Save(SerializableObject[] levelObjects, string path)
    {
        if (levelObjects == null || levelObjects.Length == 0 || string.IsNullOrEmpty(path))
            return;

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            _serializer.Serialize(fs, levelObjects);
        }
    }

    public static SerializableObject[] Load(string path)
    {
        if (!File.Exists(path))
            return null;

        SerializableObject[] result;

        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            result = (SerializableObject[])_serializer.Deserialize(fs);
        }

        return result;
    }
}
