using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;
using UnityEditor.AnimatedValues;
using UnityEngine.AI;

namespace WayPointsGenerator
{
    public class WayPointsGeneratorWindow : EditorWindow
    {
        #region Variables

        private string _nameWayPoint = "WayPoint";
        private GameObject _prefabWayPoint;
        private GameObject _rootGameObject;
        private int _numberOfWayPoints = 5;
        private float _patrolRadius = 10f;
        private bool _ignoreAxis;
        private bool _ignoreX;
        private bool _ignoreY;
        private bool _ignoreZ;
        private bool _enableStandAtPoint;
        private float _maxstandAtPointTime = 5f;
        private float _minstandAtPointTime = 2f;

        private GameObject[] _wayPoints;

        #endregion

        #region Methods
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<WayPointsGeneratorWindow>();
        }

        public void OnEnable()
        {
        }

        private void OnGUI()
        {
            _nameWayPoint = EditorGUILayout.TextField("Название точки", _nameWayPoint, GUILayout.MaxWidth(600));
            _prefabWayPoint = (GameObject)EditorGUILayout.ObjectField("Prefab путевой точки", _prefabWayPoint, typeof(GameObject), false, GUILayout.MaxWidth(600));
            _rootGameObject = (GameObject)EditorGUILayout.ObjectField("Объект генерации", _rootGameObject, typeof(GameObject), true, GUILayout.MaxWidth(600));
            _numberOfWayPoints = (int)EditorGUILayout.Slider("Количество точек маршрута", _numberOfWayPoints, 1, 1000, GUILayout.MaxWidth(600));
            _patrolRadius = EditorGUILayout.Slider("Радиус генерации относительно объекта", _patrolRadius, 10f, 100f, GUILayout.MaxWidth(600));
            _enableStandAtPoint = EditorGUILayout.ToggleLeft("Останавливаться на точках", _enableStandAtPoint);
            if (_enableStandAtPoint)
            {
                _minstandAtPointTime = EditorGUILayout.Slider("Минимальное время остановки", _minstandAtPointTime, 0f, 10f, GUILayout.MaxWidth(600));
                _maxstandAtPointTime = EditorGUILayout.Slider("Максимальное время остановки", _maxstandAtPointTime, 0f, 10f, GUILayout.MaxWidth(600));
            }
            _ignoreAxis = EditorGUILayout.BeginToggleGroup("Игнорировать координаты по координатам", _ignoreAxis);
            _ignoreX = EditorGUILayout.ToggleLeft("Игнорировать по X", _ignoreX);
            _ignoreY = EditorGUILayout.ToggleLeft("Игнорировать по Y", _ignoreY);
            _ignoreZ = EditorGUILayout.ToggleLeft("Игнорировать по Z", _ignoreZ);
            EditorGUILayout.EndToggleGroup();

            if (GUILayout.Button("Сгенерировать путевые точки", GUILayout.MaxWidth(600)))
            {
                if (_prefabWayPoint == null)
                {
                    Debug.LogError("Не выбран Prefab путевой точки");
                }
                else if (_rootGameObject == null)
                {
                    Debug.LogError("Не выбран объект, для которого генерируются путевые точки");
                }
                else
                    GenerateWayPoints();
            }                
        }

        private void GenerateWayPoints()
        {
            _wayPoints = new GameObject[_numberOfWayPoints];

            for (int i = 0; i < _numberOfWayPoints; i++)
            {
                GameObject tempObj = Instantiate(_prefabWayPoint);

                tempObj.name = string.Format("{0}_{1} ({2})", _nameWayPoint, _rootGameObject.name, i);

                Vector3 tempWPPosition = Random.insideUnitSphere * _patrolRadius;

                if (_ignoreAxis)
                {
                    if (_ignoreX)
                        tempWPPosition.x = _rootGameObject.transform.position.x;
                    if (_ignoreY)
                        tempWPPosition.y = _rootGameObject.transform.position.y;
                    if (_ignoreZ)
                        tempWPPosition.z = _rootGameObject.transform.position.z;
                }
                
                try
                {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(tempWPPosition, out hit, _patrolRadius * 0.5f, NavMesh.AllAreas))
                    {
                        tempObj.transform.position = hit.position;
                    }
                }
                catch
                {
                    Debug.LogError("Произошла ошибка. Возможно отсутствует карта навигации.");
                }

                if (_enableStandAtPoint)
                    tempObj.GetComponent<FPS.WayPoint>().WaitTime = Random.Range(_minstandAtPointTime, _maxstandAtPointTime);

                _wayPoints[i] = tempObj;
            }

        }
        #endregion
    }
}


