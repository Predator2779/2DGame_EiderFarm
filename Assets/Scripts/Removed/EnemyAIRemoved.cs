// using System.Collections;
// using Player;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// [RequireComponent(typeof(Person))]
// [RequireComponent(typeof(CircleCollider2D))]
// public class EnemyAI : MonoBehaviour
// {
//     [SerializeField] private EnemyStates _curState;
//     [SerializeField] private float _requirePlayerDistance;
//     [SerializeField] private float _requireFlagDistance;
//     [SerializeField] private float _idleTime;
//     [SerializeField] private float _patrolTime;
//     [SerializeField] private float _changeDirTime;
//     [SerializeField] private PersonAnimate _personAnimate;
//
//     private Person _person;
//     private Transform _player;
//     private Transform _flag;
//     // private Transform _gHouses;
//     private bool _isCorou;
//     private bool _isCorou2;
//     private bool _isPatrol;
//     private float _corouTime;
//     private Vector2 _curDirection;
//
//     private void Start() => SetNullableFields();
//     private void OnValidate() => SetNullableFields();
//     private void Update() => StateMachine();
//
//     private void SetNullableFields()
//     {
//         _person ??= GetComponent<Person>();
//         _curDirection = GetRandomDirection();
//     }
//
//     private void StateMachine()
//     {
//         CheckConditions();
//
//         switch (_curState)
//         {
//             case EnemyStates.Idle:
//                 break;
//             case EnemyStates.Patrol:
//                 Patrol();
//                 break;
//             case EnemyStates.Run:
//                 Run(_curDirection);
//                 break;
//         }
//     }
//
//     private void CheckConditions()
//     {
//         // if (!_isCorou2) StartCoroutine(ChangeDirection(GetRandomDirection()));
//
//         if (_player != null)
//         {
//             var distance = Vector2.Distance(
//                     transform.position,
//                     _player.position);
//
//             if (distance < _requirePlayerDistance) // while?
//             {
//                 _curDirection = GetOppositeDirection(_player.position, false);
//                 _curState = EnemyStates.Run;
//                 return;
//             }
//         }
//
//         if (_flag != null)
//         {
//             var distance = Vector2.Distance(
//                     transform.position,
//                     _flag.position);
//
//             if (distance < _requireFlagDistance) // while?
//             {
//                 StopCoroutine(ChangeDirection(GetRandomDirection()));
//                 StartCoroutine(ChangeDirection(GetOppositeDirection(_flag.position, true)));
//                 _curState = EnemyStates.Run;
//                 return;
//             }
//         }
//
//         if (!_isCorou) StartCoroutine(ChangePatrolState());
//
//         // if (_flag != null)
//         // {
//         //     if (TryGetNearFlag(out Vector2 nearFlag)) // while?
//         //     {
//         //         RunAwayFlags(nearFlag);
//         //         return;
//         //     }
//         // }
//
//         // if (_gHouses != null)
//         // {
//         //     // patrol around gHouse
//         // }
//
//         //  patrol
//     }
//
//     // private bool TryGetNearFlag(out Vector2 nearFlag)
//     // {
//     //     float minDistance = _requireFlagDistance;
//     //
//     //     foreach (var flag in _flags)
//     //     {
//     //         var distance = Vector2.Distance(transform.position, flag.position);
//     //
//     //         if (distance >= minDistance) continue;
//     //
//     //         minDistance = distance;
//     //         nearFlag = flag.position;
//     //     }
//     //
//     //     nearFlag = default;
//     //     return minDistance != _requireFlagDistance;
//     // }
//
//     private IEnumerator ChangeDirection(Vector2 direction)
//     {
//         _isCorou2 = true;
//
//         _curDirection = direction;
//
//         yield return new WaitForSeconds(_changeDirTime);
//
//         _isCorou2 = false;
//     }
//
//     private IEnumerator ChangePatrolState()
//     {
//         _isCorou = true;
//
//         if (Random.Range(0, 2) <= 0)
//         {
//             _curState = EnemyStates.Patrol;
//             _corouTime = _patrolTime;
//         }
//         else
//         {
//             _curState = EnemyStates.Idle;
//             _corouTime = _idleTime;
//         }
//
//         yield return new WaitForSeconds(_corouTime);
//
//         _isCorou = false;
//     }
//
//     private void Patrol()
//     {
//         if (!_isCorou2) StartCoroutine(ChangeDirection(GetRandomDirection()));
//
//         Walk(_curDirection);
//     }
//
//     private Vector2 GetOppositeDirection(Vector2 direction, bool isRandomDir)
//     {
//         float angle = 180;
//
//         if (isRandomDir) angle = Random.Range(90, 270);
//
//         return (Quaternion.AngleAxis(angle, Vector3.forward) * direction).normalized;
//     }
//
//     private Vector2 GetRandomDirection()
//     {
//         float angle = Random.Range(0, 360);
//
//         return (Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right).normalized;
//     }
//
//     private void Walk(Vector2 direction) => _person.Walk(direction);
//     private void Run(Vector2 direction) => _person.Run(direction);
//
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player")) _player = other.transform;
//
//         if (other.TryGetComponent(out Flag flag) && _flag != flag)
//             _flag = flag.transform;
//     }
//
//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player")) _player = null;
//
//         if (other.TryGetComponent(out Flag flag) && _flag == flag)
//             _flag = null;
//     }
//
//     private void OnCollisionEnter2D(Collision2D other)
//     {
//         StopCoroutine(ChangeDirection(GetRandomDirection()));
//         StartCoroutine(ChangeDirection(GetOppositeDirection(other.transform.position, false)));
//     }
//
//     private enum EnemyStates
//     {
//         Idle,
//         Patrol,
//         Run
//     }
// }