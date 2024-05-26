using System.Collections;
using System.Collections.Generic;
using Building;
using Building.Constructions;
using Characters.PathFinding;
using Economy;
using General;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.AI
{
    [RequireComponent(typeof(Employee))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EmployeeAI : WalkerAI
    {
        [Header("Service:")]
        [SerializeField] private EmployeeStates _currentEmployeeState;

        [SerializeField] private int _maxDistWalkable;

        [Space] [Header("Settings:")]
        [SerializeField] [Range(1, 100)] private int _fluffCapacity;

        [Space] [Header("Path Finding:")]
        [SerializeField] private PathFinder.TypeFind _findAlgorithm;

        [SerializeField] private float _radius;
        [SerializeField] private float _walkTime;
        [SerializeField] private float _distance; //

        private NavMeshAgent _agent;
        private Construction _currentCleaner;
        private BuildStorage _currentHouse;
        private BuildStorage _currentStorage;
        private BuildingsPull _pull;
        private Employee _employee;
        private PathFinder _pathFinder;
        private Vector2 _target;
        [SerializeField] private Transform _subTarget; //
        private List<Vector2> _path = new List<Vector2>();
        private int _index;

        private void Start() => Initialize();
        private void OnValidate() => Initialize();
        private void FixedUpdate() => StateExecute();

        private void Initialize()
        {
            _agent = GetComponent<NavMeshAgent>();
            _pull ??= FindObjectOfType<BuildingsPull>();
            _employee ??= GetComponent<Employee>();
            _pathFinder ??= GetComponent<PathFinder>();

            ResetPath();
        }

        private void CheckConditions()
        {
            if (CountUncleanFluff() < _fluffCapacity &&
                CountCleanFluff() <= 0 &&
                CanPickFluff())
            {
                SetTarget(_currentHouse.gameObject);
                _currentEmployeeState = EmployeeStates.Picking;
                return;
            }

            if (CountCleanFluff() < CountUncleanFluff() &&
                CountUncleanFluff() > 0 &&
                CanRecycle())
            {
                SetTarget(_currentCleaner.gameObject);
                _currentEmployeeState = EmployeeStates.Recycling;
                return;
            }

            if (CountCleanFluff() > 0 && CanTransportable())
            {
                SetTarget(_currentStorage.gameObject);
                _currentEmployeeState = EmployeeStates.Transportation;
                return;
            }

            _currentEmployeeState = EmployeeStates.Patrol;
        }

        private void StateExecute()
        {
            switch (_currentEmployeeState)
            {
                case EmployeeStates.Idle:
                    Idle();
                    break;
                case EmployeeStates.Picking:
                    Picking();
                    break;
                case EmployeeStates.Recycling:
                    Recycling();
                    break;
                case EmployeeStates.Transportation:
                    Transportation();
                    break;
                case EmployeeStates.Patrol:
                    Idle();
                    break;
            }
        }

        private void Idle()
        {
            StopMove();
            CheckConditions();
        }

        private void Picking()
        {
            if (_currentHouse == null ||
                _currentHouse.GetFluffCount() <= 0 ||
                CountUncleanFluff() > _fluffCapacity)
            {
                CheckConditions();
                return;
            }

            WalkToTarget();
        }

        private void Recycling()
        {
            if (_currentCleaner == null ||
                CountCleanFluff() >= _fluffCapacity)
            {
                CheckConditions();
                return;
            }

            WalkToTarget();
        }

        private void Transportation()
        {
            if (_currentStorage == null ||
                CountCleanFluff() <= 0)
            {
                CheckConditions();
                return;
            }

            WalkToTarget();
        }
        
        private void WalkToTarget() => _agent.SetDestination(_target);
        
        private void StopMove()
        {
            _personAnimate.Walk(_target, false);
            StopSound();
        }
        
        private IEnumerator WalkTime()
        {
            yield return new WaitForSeconds(_walkTime);
            SetPath();
        }
        
        private void SetTarget(GameObject target)
        {
            if (target.TryGetComponent(out Construction construction))
            {
                var entryPoint = construction.GetEntryPoint();

                if (entryPoint != null)
                {
                    _target = entryPoint.position;
                    _subTarget = entryPoint;
                    return;
                }
            }

            _target = target.transform.position;
            _subTarget = target.transform;
        }

        private void SetPath()
        {
            if (_pathFinder.IsWorked()) return;

            if (!_pathFinder.IsFinded() && _path == null)
            {
                ResetPath();

                _pathFinder.Initialize(
                        transform.position,
                        _target,
                        _findAlgorithm,
                        _radius,
                        _employee.GetName());
            }
            else if (_pathFinder.IsFinded())
            {
                _path = _pathFinder.GetPath();
                _index = _path.Count - 1;
                _pathFinder.Deinitialize();
            }
        }

        private bool IsNearby(Vector2 target)
        {
            var distance = Vector2.Distance(transform.position, target);

            return distance <= _radius * 1.5f;;
        }

        private int CountUncleanFluff() => TryGetBunch(GlobalConstants.UncleanedFluff)?.GetCount() ?? 0;
        private int CountCleanFluff() => TryGetBunch(GlobalConstants.CleanedFluff)?.GetCount() ?? 0;

        private ItemBunch TryGetBunch(string name) => _employee.GetInventory().TryGetBunch(
                name, out ItemBunch bunch)
                ? bunch
                : null;

        private bool CanPickFluff()
        {
            var gagaHouses = _pull.GagaHouses;

            foreach (var house in gagaHouses)
            {
                var bMenu = house.GetBuildMenu();

                if (!bMenu.IsBuilded ||
                    !bMenu.GetBuilding().TryGetComponent(out BuildStorage storage) ||
                    house.IsOccupied() ||
                    storage.GetFluffCount() <= 0) continue;

                _currentHouse = storage;
                return true;
            }

            return false;
        }

        private bool CanRecycle()
        {
            var cleaners = _pull.Cleaners;

            foreach (var cleaner in cleaners)
            {
                if (!cleaner.GetBuildMenu().IsBuilded ||
                    cleaner.IsOccupied() ||
                    cleaner.GetBuildMenu().GetBuilding().typeConstruction != GlobalTypes.TypeBuildings.FluffCleaner 
                    || CountUncleanFluff() < 2)
                    continue;

                _currentCleaner = cleaner.GetBuildMenu().GetBuilding();
                return true;
            }

            return false;
        }

        private bool CanTransportable()
        {
            var storages = _pull.Storages;

            foreach (var storage in storages)
            {
                var bMenu = storage.GetBuildMenu();

                if (!bMenu.IsBuilded ||
                    storage.IsOccupied() ||
                    !bMenu.GetBuilding().TryGetComponent(out BuildStorage bStorage))
                    continue;

                _currentStorage = bStorage;
                return true;
            }

            return false;
        }

        private void ResetPath()
        {
            _pathFinder.Deinitialize();
            _path = null;
        }

        private enum EmployeeStates
        {
            Idle,
            Picking,
            Recycling,
            Transportation,
            Patrol
        }
    }
}