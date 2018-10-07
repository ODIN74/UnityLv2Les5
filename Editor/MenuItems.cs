using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WayPointsGenerator
{
    public class MenuItems
    {
        [MenuItem("Window/Experimental/WayPointsGenerator &#p",false,100000)]
        [MenuItem("CONTEXT/EnemyBot/WayPointsGenerator")]
        public static void OpenWayPointsGenerator()
        {
            WayPointsGeneratorWindow.ShowWindow();
        }
    }
}


