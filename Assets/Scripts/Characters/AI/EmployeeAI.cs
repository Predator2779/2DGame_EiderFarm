using System;
using System.Collections;
using System.Collections.Generic;
using Building;
using Building.Constructions;
using Characters.AI;
using Economy;
using General;
using UnityEngine;

namespace Characters.Enemy
{
    [RequireComponent(typeof(Employee))]
    [RequireComponent(typeof(PathFinder))]
    public class EmployeeAI : WalkerAI
    {
        [Header("Service")]
        [SerializeField] private EmployeeStates _currentEmployeeState;

        [SerializeField] private int _maxDistance;
        [SerializeField] private float _idleDelay;

        [Space] [Header("Settings:")]
        [SerializeField] [Range(1, 100)] private int _fluffCapacity;

        [SerializeField] private BuildingsPull _pull;
        [SerializeField] private BuildStorage _currentHouse;
        [SerializeField] private Transform _currentCleaner;
        [SerializeField] private Inventory _currentStorage;

        private PathFinder _pathFinder;
        private Employee _employee;
        private Rigidbody2D _rbody;
        private Vector2 _target;
        private List<Vector2> _path = new();
        private bool _isDelayed;
        private int _index;

        private void Start() => Initialize();
        private void OnValidate() => Initialize();
        private void FixedUpdate() => StateExecute();

        private void Initialize()
        {
            _pull ??= FindObjectOfType<BuildingsPull>();
            _employee ??= GetComponent<Employee>();
            _rbody ??= GetComponent<Rigidbody2D>();
            _pathFinder ??= GetComponent<PathFinder>();
        }

        private void CheckConditions()
        {
            print($"IsFull: {IsFull()}, CanRecycle: {CanRecycle()}, CountUncleanFluff: {CountUncleanFluff()}");
            if (!IsFull() &&
                CanPickFluff() &&
                CountCleanFluff() <= 0 &&
                CountUncleanFluff() < _fluffCapacity)
            {
                SetTarget(_currentHouse.gameObject);
                _currentEmployeeState = EmployeeStates.Picking;
                print("picking");
                return;
            }

            if (IsFull() && CanRecycle() && CountUncleanFluff() > 0)
            {
                SetTarget(_currentCleaner.gameObject);
                _currentEmployeeState = EmployeeStates.Recycling;
                print("recycle");
                return;
            }
            
            if (CountCleanFluff() > 0 && CanTransportable())
            {
                SetTarget(_currentStorage.gameObject);
                _currentEmployeeState = EmployeeStates.Transportation;
                print("transportable");
                return;
            }
            
            _currentEmployeeState = EmployeeStates.Patrol;
            print("idle");
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
                case EmployeeStates.SideStep:
                    SideStep();
                    break;
            }
        }

        private void WalkToTarget()
        {
            if (Vector2.Distance(transform.position, _target) > _maxDistance) return;

            if (!IsDestination(transform.position, _target))
            {
                if (_index > 0)
                {
                    if (_path == null) return;

                    if (!IsDestination(transform.position, _path[_index]))
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
            _path = _pathFinder.GetPath(target);
            _index = _path.Count - 1;
        }

        private bool IsDestination(Vector2 first, Vector2 second) => (int)Vector2.Distance(first, second) == 0;
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

                _currentCleaner = cleaner.transform;
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
                    !bMenu.GetBuilding().TryGetComponent(out Inventory inventory) ||
                    inventory.GetFreeSpace() > 0) continue;

                _currentStorage = inventory;
                return true;
            }

            return false;
        }

        private void Idle()
        {
            _personAnimate.Walk(_target, false);
            CheckConditions();//
            // if (!_isDelayed) StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            _isDelayed = true;
            yield return new WaitForSeconds(_idleDelay);
            CheckConditions();
            _isDelayed = false;
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

            if (IsDestination(transform.position, _currentStorage.transform.position))
            {
                _currentStorage.Exchange(
                        _employee.GetInventory(),
                        _currentStorage,
                        TryGetBunch(GlobalConstants.CleanedFluff));

                CheckConditions();
            }

            WalkToTarget();
        }

        private void SideStep()
        {
            /// Добавить проверку. (если некоторое время не двигается -> смена пути)

            CheckConditions();

            if (!IsDestination(transform.position, _target))
                WalkToTarget();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            SetPath(_target);
            _currentEmployeeState = EmployeeStates.SideStep;
        }

        private enum EmployeeStates
        {
            Idle,
            Picking,
            Recycling,
            Transportation,
            SideStep,
            Patrol
        }
    }
}