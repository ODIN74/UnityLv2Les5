using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SceneCleaner
{
    public class SceneCleaner
    {
        [MenuItem("Window/Experimental/Clean Scene")]
        [MenuItem("Assets/Clean Scene")]
        private static void Clean()
        {
            Transform[] objects = Object.FindObjectsOfType<Transform>();

            foreach (var obj in objects)
            {
                Object.DestroyImmediate(obj.gameObject);
            }
        }
    }
}


