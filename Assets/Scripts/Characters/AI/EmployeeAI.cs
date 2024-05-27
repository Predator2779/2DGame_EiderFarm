using System;
using Building;
using Building.Constructions;
using Economy;
using General;
using Other;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.AI
{
    [RequireComponent(typeof(Employee))]
    public class EmployeeAI : WalkerAI
    {
        [Header("Service:")]
        [SerializeField] private EmployeeStates _currentEmployeeState;

        [SerializeField] private NavMeshAgent _agent;

        [Space] [Header("Settings:")]
        [SerializeField] [Range(1, 100)] private int _fluffCapacity;

        [SerializeField] private float _radius;

        [Space] [Header("Thinking:")]
        [SerializeField] private SpriteRenderer _mindCloud;
        [SerializeField] private SpriteRenderer _thought;
        [SerializeField] private Thought[] _thoughts;

        private Construction _currentCleaner;
        private BuildStorage _currentHouse;
        private BuildStorage _currentStorage;
        private BuildingsPull _pull;
        private Employee _employee;
        private Vector2 _target;

        private void Start() => Initialize();
        private void OnValidate() => Initialize();
        private void FixedUpdate() => StateExecute();

        private void Initialize()
        {
            _pull ??= FindObjectOfType<BuildingsPull>();
            _employee ??= GetComponent<Employee>();
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
                    ChangeMindCloud(_currentEmployeeState);
                    Picking();
                    break;
                case EmployeeStates.Recycling:
                    ChangeMindCloud(_currentEmployeeState);
                    Recycling();
                    break;
                case EmployeeStates.Transportation:
                    ChangeMindCloud(_currentEmployeeState);
                    Transportation();
                    break;
                case EmployeeStates.Patrol:
                    Idle();
                    break;
                case EmployeeStates.Walk:
                    Walk();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangeMindCloud(EmployeeStates state)
        {
            print("base state: " + state);
            _mindCloud.gameObject.SetActive(true);

            foreach (var t in _thoughts)
            {
                print(t.state);
                if (t.state == state) _thought.sprite = t.sprite;
            }
        }

        private void Idle()
        {
            _mindCloud.gameObject.SetActive(false);
            StopMove();
            CheckConditions();
        }

        private void Walk()
        {
            if (IsNearby(_target))
            {
                _currentEmployeeState = EmployeeStates.Idle;
                return;
            }

            base.WalkAnimation(_target - (Vector2) transform.position);
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

        private bool IsNearby(Vector2 target)
        {
            var distance = Vector2.Distance(transform.position, target);
            return distance <= _radius * 1.5f;
            ;
        }

        private void WalkToTarget()
        {
            _agent.SetDestination(_target);
            _currentEmployeeState = EmployeeStates.Walk;
        }

        private void StopMove()
        {
            _personAnimate.Walk(_target, false);
            StopSound();
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

        private int CountUncleanFluff() => TryGetBunch(GlobalConstants.UncleanedFluff)?.GetCount() ?? 0;
        private int CountCleanFluff() => TryGetBunch(GlobalConstants.CleanedFluff)?.GetCount() ?? 0;

        private ItemBunch TryGetBunch(string nameBunch) => _employee.GetInventory().TryGetBunch(
                nameBunch, out ItemBunch bunch)
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

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

#endif

        public enum EmployeeStates
        {
            Idle,
            Picking,
            Recycling,
            Transportation,
            Patrol,
            Walk
        }
    }
}