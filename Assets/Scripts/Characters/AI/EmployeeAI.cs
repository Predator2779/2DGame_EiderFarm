using System;
using Building;
using Building.Constructions;
using Economy;
using General;
using Other;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

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
        [SerializeField] private float _requireDistance;

        [Space] [Header("Thinking:")]
        [SerializeField] private SpriteRenderer _mindCloud;
        [SerializeField] private SpriteRenderer _thought;
        [SerializeField] private Thought[] _thoughts;

        private Construction _currentCleaner;
        private BuildStorage _currentHouse;
        private BuildStorage _currentStorage;
        private BuildingsPull _pull;
        private Employee _employee;
        private Transform _target;

        private void Start() => Initialize();
        // private void OnValidate() => Initialize();
        private void FixedUpdate() => Execute();

        private void Initialize()
        {
            _pull ??= FindObjectOfType<BuildingsPull>();
            _employee ??= GetComponent<Employee>();
            _currentEmployeeState = EmployeeStates.Idle;
        }

        private void Execute()
        {
            if (!IsDestination()) Walk();
            else StateExecute();
        }

        private void CheckConditions()
        {
            if (CanPick())
            {
                SetTarget(_currentHouse.gameObject);
                _currentEmployeeState = EmployeeStates.Picking;
                return;
            }

            if (CanRecycle())
            {
                SetTarget(_currentCleaner.gameObject);
                _currentEmployeeState = EmployeeStates.Recycling;
                return;
            }

            if (CanTransportation())
            {
                SetTarget(_currentStorage.gameObject);
                _currentEmployeeState = EmployeeStates.Transportation;
                return;
            }

            _currentEmployeeState = EmployeeStates.Idle;
        }

        // собирает, пока не заполнится до максимума и при этом нет очищенного
        private bool CanPick() => CountUncleanFluff() < _fluffCapacity &&
                                  CountCleanFluff() <= 0 &&
                                  HasUncleanFluff();

        // обрабатывает, пока чистый не заполнится до максимума, либо пока не кончится неочищенный
        private bool CanRecycle() => (CountCleanFluff() < _fluffCapacity ||
                                      CountUncleanFluff() > 0) &&
                                     HasCleaner();

        // стоит у склада, пока не выгрузит весь очищенный пух
        private bool CanTransportation() => CountCleanFluff() > 0 &&
                                            HasStorage();


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
            }
        }

        private void ChangeMindCloud(EmployeeStates state)
        {
            _mindCloud.gameObject.SetActive(true);

            foreach (var t in _thoughts)
                if (t.state == state)
                    _thought.sprite = t.sprite;
        }

        private void Idle()
        {
            _mindCloud.gameObject.SetActive(false);
            StopMove();
            CheckConditions();
        }

        private void Walk() => base.WalkAnimation(_target.position - transform.position);

        private void Picking()
        {
            if (_currentHouse == null ||
                _currentHouse.GetFluffCount() <= 0
                    // || CountUncleanFluff() > _fluffCapacity
            )
                _currentEmployeeState = EmployeeStates.Idle;
            print("собираю...");
        }

        private void Recycling()
        {
            if (_currentCleaner == null ||
                CountUncleanFluff() <= 0)
                _currentEmployeeState = EmployeeStates.Idle;
            print("обрабатываю...");
        }

        private void Transportation()
        {
            if (_currentStorage == null ||
                CountCleanFluff() <= 0)
                _currentEmployeeState = EmployeeStates.Idle;
            print("транспортирую...");
        }

        private bool IsDestination() =>
                _target == null || Vector2.Distance(transform.position, _target.position) <= _requireDistance;

        private int CountUncleanFluff() => TryGetBunch(GlobalConstants.UncleanedFluff)?.GetCount() ?? 0;
        private int CountCleanFluff() => TryGetBunch(GlobalConstants.CleanedFluff)?.GetCount() ?? 0;

        private ItemBunch TryGetBunch(string nameBunch) => _employee.GetInventory().TryGetBunch(
                nameBunch, out ItemBunch bunch)
                ? bunch
                : null;

        private void StopMove()
        {
            var direction = Vector2.zero;

            if (_target) direction = _target.position;

            _personAnimate.Walk(direction, false);
            StopSound();
        }

        private void SetTarget(GameObject target)
        {
            if (target.TryGetComponent(out Construction construction))
            {
                var entryPoint = construction.GetEntryPoint();
                if (entryPoint != null)
                {
                    _target = entryPoint;
                    return;
                }
            }

            _target = target.transform;
            _agent.SetDestination(_target.position);
            ChangeMindCloud(_currentEmployeeState);
        }

        private bool HasUncleanFluff()
        {
            var gagaHouses = _pull.GagaHouses;

            foreach (var house in gagaHouses)
            {
                var bMenu = house.GetBuildMenu();

                if (bMenu.IsBuilded &&
                    bMenu.GetBuilding().TryGetComponent(out BuildStorage storage) &&
                    !house.IsOccupied() &&
                    storage.GetFluffCount() > 0)
                {
                    _currentHouse = storage;
                    return true;
                }
            }

            return false;
        }

        private bool HasCleaner()
        {
            var cleaners = _pull.Cleaners;

            foreach (var cleaner in cleaners)
            {
                if (cleaner.GetBuildMenu().IsBuilded &&
                    !cleaner.IsOccupied() &&
                    cleaner.GetBuildMenu().GetBuilding().typeConstruction == GlobalTypes.TypeBuildings.FluffCleaner)
                {
                    _currentCleaner = cleaner.GetBuildMenu().GetBuilding();
                    return true;
                }
            }

            return false;
        }

        private bool HasStorage()
        {
            var storages = _pull.Storages;

            foreach (var storage in storages)
            {
                var bMenu = storage.GetBuildMenu();

                if (bMenu.IsBuilded &&
                    !storage.IsOccupied() &&
                    bMenu.GetBuilding().TryGetComponent(out BuildStorage bStorage))
                {
                    _currentStorage = bStorage;
                    return true;
                }
            }

            return false;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _requireDistance);
        }

#endif

        public enum EmployeeStates
        {
            Idle,
            Picking,
            Recycling,
            Transportation
        }
    }
}