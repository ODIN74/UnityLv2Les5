using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelGenerator : EditorWindow {

    private float _sizeX = 100f;
    private float _sizeY = 10f;
    private float _sizeZ = 100f;

    [MenuItem("Window/Experimental/Level Generator", false, 100000)]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<LevelGenerator>();
    }

    private void OnGUI()
    {
        _sizeX = EditorGUILayout.FloatField("Размер уровня по X", _sizeX, GUILayout.MaxWidth(600));
        _sizeZ = EditorGUILayout.FloatField("Размер уровня по Z", _sizeZ, GUILayout.MaxWidth(600));
        _sizeY = EditorGUILayout.FloatField("Высота стен", _sizeY, GUILayout.MaxWidth(600));

        if (GUILayout.Button("Сгенерировать уровень", GUILayout.MaxWidth(600)))
        {
            Generate();
        }

    }

    private void Generate()
    {
        GameObject _mainPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        _mainPlane.transform.position = Vector3.zero;
        _mainPlane.transform.rotation = Quaternion.identity;
        _mainPlane.transform.localScale = new Vector3(_sizeX / 10f, 1f, _sizeZ / 10f);

        GameObject[] outsideWalls = new GameObject[4];

        for(int i = 0; i < 4; i++)
        {
            outsideWalls[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);

            switch(i)
            {
                case 0:
                    outsideWalls[i].transform.localScale = new Vector3(_sizeX - 2f, _sizeY, 1f);
                    outsideWalls[i].transform.position = new Vector3(0f, _sizeY / 2, -_sizeZ / 2 + 0.5f);
                    break;

                case 1:
                        outsideWalls[i].transform.localScale = new Vector3(1f, _sizeY, _sizeZ - 2f);
                        outsideWalls[i].transform.position = new Vector3(-_sizeX / 2 + 0.5f, _sizeY / 2, 0f);
                    break;

                case 2:
                    outsideWalls[i].transform.localScale = new Vector3(_sizeX - 2f, _sizeY, 1f);
                    outsideWalls[i].transform.position = new Vector3(0f, _sizeY / 2, _sizeZ / 2 - 0.5f);
                    break;
                case 3:
                        outsideWalls[i].transform.localScale = new Vector3(1f, _sizeY, _sizeZ - 2f);
                        outsideWalls[i].transform.position = new Vector3(_sizeX / 2 - 0.5f, _sizeY / 2, 0f);
                    break;
            }
        }

    }

}
