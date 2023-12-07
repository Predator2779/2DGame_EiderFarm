using System.Collections;
using System.Collections.Generic;
using Building;
using Characters.AI;
using Economy;
using General;
using UnityEngine;

namespace Characters.Enemy
{
    [RequireComponent(typeof(Employee))]
    [RequireComponent(typeof(PathFinder))]
    public class EmployeeAI : PatrollerAI
    {
        [Header("Service")]
        [SerializeField] private EmployeeStates _currentEmployeeState;

        [Space] [Header("Settings:")]
        [SerializeField] [Range(1, 100)] private int _fluffCapacity;

        [SerializeField] private BuildingsPull _pull;
        private BuildStorage _currentHouse;
        private Transform _currentCleaner;
        private Inventory _currentStorage;
        private PathFinder _pathFinder;
        private Employee _employee;
        private Vector2 _target;
        private List<Vector2> _path = new();
        private int _index;
        private bool _isIdle;

        protected override void Update()
        {
        }

        protected override void Initialize()
        {
            _pull ??= FindObjectOfType<BuildingsPull>();
            _employee ??= GetComponent<Employee>();
            _pathFinder ??= GetComponent<PathFinder>();

            SetPath(_target);
        }

        protected override void CheckConditions()
        {
            if (!IsFull() && CanPickFluff() && _currentEmployeeState == EmployeeStates.Picking)
            {
                SetPath(_currentHouse.transform.position);
                _currentEmployeeState = EmployeeStates.Picking;
            }

            if (!IsFull() && CanRecycle() && CountCleanFluff() > 0 && _currentEmployeeState == EmployeeStates.Recycling)
            {
                SetPath(_currentCleaner.transform.position);
                _currentEmployeeState = EmployeeStates.Recycling;
                return;
            }

            if (CountCleanFluff() > 0 && CanTransportable())
            {
                SetPath(_currentStorage.transform.position);
                _currentEmployeeState = EmployeeStates.Transportation;
                return;
            }

            _currentEmployeeState = EmployeeStates.Idle; //patrol
        }

        protected override void StateExecute()
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
                    break;
            }
        }

        private void WalkToTarget()
        {
            if (IsDestination(transform.position, _target))
            {
                Idle();
                return;
            }

            if (_index >= 0)
            {
                if (!IsDestination(transform.position, _path[_index]))
                {
                    var target = _path[_index] - (Vector2)transform.position;
                    Walk(target.normalized);
                    return;
                }

                _index--;
                return;
            }

            SetPath(_target);
        }

        private void SetPath(Vector2 target)
        {
            _path = _pathFinder.GetPath(target);
            _index = _path.Count - 1;
        }

        private bool IsDestination(Vector2 first, Vector2 second) => (int)Vector2.Distance(first, second) == 0;
        private bool IsFull() => CountUncleanFluff() >= _employee.GetInventory().GetLimit();
        private int CountUncleanFluff() => TryGetBunch(GlobalConstants.UncleanedFluff)?.GetCount() ?? 0;
        private int CountCleanFluff() => TryGetBunch(GlobalConstants.CleanedFluff)?.GetCount() ?? 0;

        private ItemBunch TryGetBunch(string name) => _employee.GetInventory().TryGetBunch(
                name, out ItemBunch bunch) ? bunch : null;

        private bool CanPickFluff()
        {
            var gagaHouses = _pull.GagaHouses;

            foreach (var house in gagaHouses)
            {
                if (!house.GetBuildMenu().IsBuilded ||
                    !house.TryGetComponent(out BuildStorage storage) ||
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
                if (!storage.GetBuildMenu().IsBuilded ||
                    !storage.GetBuildMenu().GetBuilding().TryGetComponent(out Inventory inventory) ||
                    inventory.GetFreeSpace() <= 0) continue;

                _currentStorage = inventory;
                return true;
            }

            return false;
        }

        private IEnumerator ConditionCoroutine()
        {
            yield return new WaitForSeconds(_idleDelay);
            CheckConditions();
        }
        
        protected override void Idle()
        {
            base.Idle();
            CheckConditions();
        }

        private void Picking()
        {
            if (_currentHouse == null || _currentHouse.GetFluffCount() <= 0)
                CheckConditions();

            WalkToTarget();
        }

        private void Recycling()
        {
            if (_currentCleaner == null || _currentHouse.GetFluffCount() <= 0)
                CheckConditions();

            WalkToTarget();
        }

        private void Transportation()
        {
            if (_currentStorage == null || _currentStorage.GetFreeSpace() <= 0)
                CheckConditions();

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

        private void Patrol()
        {
            base.StateExecute();
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