using System.Collections.Generic;
using Building;
using Building.Constructions;
using Economy;
using General;
using UnityEngine;

namespace Characters.AI
{
    [RequireComponent(typeof(Employee))]
    [RequireComponent(typeof(PathFinder))]
    [RequireComponent(typeof(PathFinder2))]
    public class EmployeeAI : WalkerAI
    {
        [Header("Service")]
        [SerializeField] private EmployeeStates _currentEmployeeState;

        [SerializeField] private int _maxDistance;

        [Space] [Header("Settings:")]
        [SerializeField] [Range(1, 100)] private int _fluffCapacity;

        [SerializeField] private float _targetF;
        [SerializeField] private float _cellF;

        private Construction _currentCleaner;
        private BuildStorage _currentHouse;
        private BuildStorage _currentStorage;
        private BuildingsPull _pull;
        private PathFinder _pathFinder;
        private PathFinder2 _pathFinder2;
        private Employee _employee;
        private Vector2 _target;
        private List<Vector2> _path = new();
        [SerializeField] private int _index;

        [SerializeField] private LayerMask _layer;

        private void Start() => Initialize();
        private void OnValidate() => Initialize();
        private void FixedUpdate() => StateExecute();

        private void Initialize()
        {
            _pull ??= FindObjectOfType<BuildingsPull>();
            _pathFinder ??= GetComponent<PathFinder>();
            _pathFinder2 ??= GetComponent<PathFinder2>();
            _employee ??= GetComponent<Employee>();
        }

        private void CheckConditions()
        {
            if (!IsFull() &&
                CanPickFluff() &&
                CountCleanFluff() <= 0 &&
                CountUncleanFluff() < _fluffCapacity)
            {
                SetTarget(_currentHouse.gameObject);
                _currentEmployeeState = EmployeeStates.Picking;
                return;
            }

            if (IsFull() && CanRecycle() && CountUncleanFluff() > 0)
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

        private void WalkToTarget()
        {
            if (Vector2.Distance(transform.position, _target) > _maxDistance) return;

            if (IsDestination(transform.position, _target) > _targetF)
            {
                if (_index > 0)
                {
                    if (_path == null) return;

                    if (IsDestination(transform.position, _path[_index]) > _cellF)
                    {
                        var direction = _path[_index] - (Vector2)transform.position;
                        Walk(direction.normalized);
                    }
                    else
                    {
                        _index--;
                    }
                }
                else SetPath(_target);
            }
            else
            {
                Idle();
            }
        }

        private void SetTarget(GameObject target)
        {
            if (target.TryGetComponent(out Construction construction))
            {
                var entryPoint = construction.GetEntryPoint();

                if (entryPoint != null)
                {
                    _target = entryPoint.position;
                    return;
                }
            }

            _target = target.transform.position;
        }

        private void SetPath(Vector2 target)
        {
            if (_pathFinder2.isWorked) return;

            if (_pathFinder2.isFinded)
            {
                _path = _pathFinder2.pathToTarget;
                _index = _path.Count - 1;
            }
            else _pathFinder2.Initialize(transform.position, _target, _layer, _targetF);
        }

        private float IsDestination(Vector2 first, Vector2 second) => Vector2.Distance(first, second);
        private bool IsFull() => CountUncleanFluff() >= _fluffCapacity;
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
                    cleaner.GetBuildMenu().GetBuilding().typeConstruction != GlobalTypes.TypeBuildings.FluffCleaner)
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
                    !bMenu.GetBuilding().TryGetComponent(out BuildStorage bStorage))
                    continue;

                _currentStorage = bStorage;
                return true;
            }

            return false;
        }

        private void Idle()
        {
            CheckConditions();

            _personAnimate.Walk(_target, false);
            StopSound();
        }

        private void Picking()
        {
            if (_currentHouse == null || _currentHouse.GetFluffCount() <= 0)
            {
                CheckConditions();
                return;
            }

            WalkToTarget();
        }

        private void Recycling()
        {
            if (_currentCleaner == null || CountUncleanFluff() <= 0)
            {
                CheckConditions();
                return;
            }

            WalkToTarget();
        }

        private void Transportation()
        {
            if (_currentStorage == null)
            {
                CheckConditions();
                return;
            }

            WalkToTarget();
            CheckConditions();
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