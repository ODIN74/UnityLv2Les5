using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace FPS
{
    public class EnemyBot : BaseObjectScene, IDamageable
    {

        private float timeFromStandToIdle = 1.0f;
        public float TimeFromStandToIdle
        {
            get { return timeFromStandToIdle; }
            private set
            {
                timeFromStandToIdle = value;
            }
        }

        private float destroyDelay = 1.0f;
        public float DestroyDelay
        {
            get { return destroyDelay; }
            private set
            {
                destroyDelay = value;
            }
        }

        [SerializeField]
        private bool enableRandomPosition = true;

        [SerializeField]
        private float patrolDistance = 10f;

        [SerializeField]
        private WayPoint[] _wayPoints;

        [SerializeField]
        private float _seekDistance = 10f;

        [SerializeField]
        private float _attackDistance = 3f;

        private float _damageDelay = 1f;
        public float DamageDelay
        {
            get { return _damageDelay; }
            private set
            {
                _damageDelay = value;
            }
        }

        [SerializeField]
        private int _damage = 10;

        [SerializeField]
        private float maxHealth;

        public float MaxHelth { get { return maxHealth; } }

        public bool IsAlive { get { return currentHealth > 0; } }

        private float currentHealth = 0;

        public float CurrentHealth { get { return currentHealth; } }

        private Animator _anim;

        private Transform _eyesTransform;

        private Vector3 _randomPosition;

        private Transform _targetTransform;

        private bool targetVisibility;

        private NavMeshAgent _agent;

        private int _currentWP = 0;

        private float _currentWPTimeout;

        protected override void Awake()
        {
            base.Awake();
            _anim = GetComponent<Animator>();
            currentHealth = maxHealth;
            Transform[] transformArray = GetComponentsInChildren<Transform>();
            foreach (var tr in transformArray)
            {
                if (tr.tag.Equals("Eyes"))
                {
                    _eyesTransform = tr;
                    break;
                }
            }

            Initialize();
        }

        private void Start()
        {
            SetTarget(PlayerModel.LocalPlayer.transform);
        }

        public void SetTarget(Transform transform)
        {
            _targetTransform = transform;
        }

        private void Initialize()
        {
            _agent = GetComponent<NavMeshAgent>();
            _randomPosition = GenerateRandomWP();
            foreach (var wp in _wayPoints)
            {
                wp.transform.SetParent(null);
            }
        }

        private Vector3 GenerateRandomWP()
        {
            Vector3 randomWP = UnityEngine.Random.insideUnitSphere * patrolDistance;

            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(transform.position + randomWP, out navMeshHit, patrolDistance * 1.5f, NavMesh.AllAreas))
                randomWP = navMeshHit.position;

            return randomWP;
        }

        private void Update()
        { 

            if (!IsAlive)
                return;

            if(_targetTransform)
            {
                float distance = Vector3.Distance(transform.position, _targetTransform.position);
                if (distance < _attackDistance)
                {
                    targetVisibility = IsTargetVisible();
                    if (targetVisibility)
                        Attack();
                }
                
                else if (distance < _seekDistance && distance < _attackDistance)
                {
                    targetVisibility = IsTargetVisible();
                    if (targetVisibility)
                        Run();
                }
                else
                    targetVisibility = false;
            }

            if (!targetVisibility)
            {

                if (enableRandomPosition)
                {
                    _agent.SetDestination(_randomPosition);
                    _anim.SetTrigger("Walk");

                    if (!_agent.hasPath)
                    {
                        _randomPosition = GenerateRandomWP();
                    }
                }
                else
                {
                    if (_wayPoints.Length > 1)
                    {
                        _agent.SetDestination(_wayPoints[_currentWP].transform.position);

                        if (!_agent.hasPath)
                        {
                            _anim.SetTrigger("Stand");

                            _currentWPTimeout += Time.deltaTime;
                            if (_currentWPTimeout >= _wayPoints[_currentWP].WaitTime)
                            {
                                _currentWPTimeout = 0;
                                _currentWP++;
                                if (_currentWP >= _wayPoints.Length)
                                    _currentWP = 0;
                            }
                        }
                        else
                            _anim.SetTrigger("Walk");
                    }
                }
            }
            if (_anim && _anim.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                StartCoroutine(FromStandToIdleDelay(timeFromStandToIdle));
        }

        private bool IsTargetVisible()
        {
            RaycastHit hit;
            if (Physics.Linecast(_eyesTransform.position, _targetTransform.position, PlayerModel.LocalPlayer.Layer))
            {
                return true;
            }
            return false;
        }

        private void Run()
        {
            _agent.speed *= 2; 
            _agent.SetDestination(_targetTransform.position);
            _anim.SetTrigger("Run");
        }

        private void Attack()
        {
            _anim.ResetTrigger("Run");
            _agent.SetDestination(transform.position);
            _anim.SetTrigger("Attack");
            StartCoroutine(AttackDelay(_damageDelay));
        }

        private IEnumerator AttackDelay(float damageDelay)
        {
            yield return new WaitForSeconds(damageDelay);
            PlayerModel.LocalPlayer.Damage(_damage);
            StopCoroutine(AttackDelay(_damageDelay));
        }

        private IEnumerator FromStandToIdleDelay(float timeFromStandToIdle)
        {
            yield return new WaitForSeconds(timeFromStandToIdle);
            _anim.SetTrigger("Idle");
            StopCoroutine(FromStandToIdleDelay(timeFromStandToIdle));
        }

        public void ApplyDamage(float damage)
        {
            if (!IsAlive)
                return;

            
            currentHealth -= damage;

            if (!IsAlive)
                Death();

            if (_anim)
            _anim.SetTrigger("Damage");
        }

        public void Death()
        {
            _anim.SetTrigger("Death");
            Destroy(gameObject, destroyDelay);
        }

        public void SetTimeFromStandToIdle(float time)
        {
            timeFromStandToIdle = time;
        }

        public void SetDestroyDelay(float time)
        {
            destroyDelay = time;
        }

        public void SetDamageDelay(float time)
        {
            _damageDelay = time;
        }
    }

}