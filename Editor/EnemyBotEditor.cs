using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FPS;


    [CustomEditor(typeof(EnemyBot))]

    public class EnemyBotEditor : Editor
    {
        private bool _inicializeData = true;
        private bool _isButtonDown;
        private float _timeFromStandToIdle;
        private float _damageDelay;
        private float _destroyDelay;
    

    public override void OnInspectorGUI()
        {
        DrawDefaultInspector();

        EnemyBot _eBot = (EnemyBot)target;

        if (_inicializeData)
        {
            _timeFromStandToIdle = _eBot.TimeFromStandToIdle;
            _damageDelay = _eBot.DamageDelay;
            _destroyDelay = _eBot.DestroyDelay;
            _inicializeData = false;
        }

        _isButtonDown = GUILayout.Toggle(_isButtonDown,"Настройки для анимации");

            if (_isButtonDown)
            {
                _timeFromStandToIdle = EditorGUILayout.FloatField("Время перехода из Stand в Idle", _timeFromStandToIdle);
                _destroyDelay = EditorGUILayout.FloatField("Время до уничтожения", _destroyDelay, GUILayout.MaxWidth(600));
                _damageDelay = EditorGUILayout.FloatField("Время задержки урона", _damageDelay, GUILayout.MaxWidth(600));

                if (GUILayout.Button("Применить настройки", EditorStyles.miniButton))
                {
                    _eBot.SetTimeFromStandToIdle(_timeFromStandToIdle);
                    _eBot.SetDamageDelay(_damageDelay);
                    _eBot.SetDestroyDelay(_destroyDelay);

                    _isButtonDown = false;
                }       
            }
        }
    }

