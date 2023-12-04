using System.Collections;
using Characters;
using Characters.AI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Charcters.AI
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class PatrollerAI : AnimalAI
    {
        [SerializeField] private EnemyStates _currentState;
        [SerializeField] private float _requirePlayerDistance;
        [SerializeField] private float _requireFlagDistance;
        [SerializeField] private float _idleTime;
        [SerializeField] private float _patrolTime;
        [SerializeField] private float _changeDirTime;

        private Transform _player;
        private Transform _flag;
        private Vector2 _currentDirection;
        private float _patrolDelay;
        private bool _isPatrol;
        private bool _canChangePatrolState;
        private bool _canChangeDir;

        private void Start() => Initialize();
        private void OnValidate() => Initialize();
        private void Update() => CheckConditions();
        private void FixedUpdate() => StateExecute();

        private void Initialize()
        {
            _currentDirection = GetRandomDirection();
            _canChangePatrolState = true;
            _canChangeDir = true;
        }

        protected override void StateExecute()
        {
            switch (_currentState)
            {
                case EnemyStates.Idle:
                    Idle();
                    break;
                case EnemyStates.Patrol:
                    Patrol();
                    break;
                case EnemyStates.Run:
                    Run(_currentDirection);
                    break;
            }
        }

        private void Patrol()
        {
            if (_canChangeDir) StartCoroutine(ChangeDirection(GetRandomDirection()));
            Walk(_currentDirection);
        }

        protected override void Idle()
        {
            _personAnimate.Walk(_currentDirection, false);
        }

        protected override void Run(Vector2 direction)
        {
            StopCoroutine(ChangeDirection(direction));
            base.Run(direction);
        }

        protected override void CheckConditions()
        {
            if (_player != null)
            {
                if (IsLessDistance(_player, _requirePlayerDistance))
                {
                    _currentDirection = GetOppositeDirection(_player.position, false);
                    _currentState = EnemyStates.Run;
                    return;
                }
            }

            if (_flag != null)
            {
                if (IsLessDistance(_flag, _requireFlagDistance))
                {
                    StopCoroutine(ChangeDirection(GetRandomDirection()));
                    StartCoroutine(ChangeDirection(GetOppositeDirection(_flag.position, true)));
                    _currentState = EnemyStates.Run;
                    return;
                }
            }

            if (_canChangePatrolState) StartCoroutine(ChangePatrolState());
        }

        private bool IsLessDistance(Transform obj, float requireDistance) =>
                Vector2.Distance(transform.position, obj.position) < requireDistance;

        private IEnumerator ChangeDirection(Vector2 direction)
        {
            _canChangeDir = false;
            _currentDirection = direction;
            yield return new WaitForSeconds(_changeDirTime);
            _canChangeDir = true;
        }

        private IEnumerator ChangePatrolState()
        {
            _canChangePatrolState = false;

            if (Random.Range(0, 2) <= 0)
            {
                _currentState = EnemyStates.Patrol;
                _patrolDelay = _patrolTime;
            }
            else
            {
                _currentState = EnemyStates.Idle;
                _patrolDelay = _idleTime;
            }

            yield return new WaitForSeconds(_patrolDelay);
            _canChangePatrolState = true;
        }

        private Vector2 GetOppositeDirection(Vector2 direction, bool isRandomDir)
        {
            float angle = 180;
            if (isRandomDir) angle = Random.Range(90, 270);

            return (Quaternion.AngleAxis(angle, Vector3.forward) * direction).normalized;
        }

        private Vector2 GetRandomDirection()
        {
            float angle = Random.Range(0, 360);

            return (Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right).normalized;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) _player = other.transform;

            if (other.TryGetComponent(out Flag flag) &&
                flag.isFlagAdded && _flag != flag)
                _flag = flag.transform;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player")) _player = null;

            if (other.TryGetComponent(out Flag flag) &&
                flag.isFlagAdded && _flag == flag)
                _flag = null;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            // развернуться при упоре в стену
            StopCoroutine(ChangeDirection(GetRandomDirection()));
            StartCoroutine(ChangeDirection(GetOppositeDirection(other.transform.position, false)));
        }

        private enum EnemyStates
        {
            Idle,
            Patrol,
            Run
        }
    }
}