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
        [SerializeField] private NavMeshAgent _navMeshPrefab;

        [Space] [Header("Settings:")] 
        [SerializeField] [Range(1, 100)] private int _fluffCapacity;

        [SerializeField] private float _requireDistance;

        [Space] [Header("Thinking:")] 
        [SerializeField] private SpriteRenderer _mindCloud;
        [SerializeField] private SpriteRenderer _thought;
        [SerializeField] private Thought[] _thoughts;

        private Following _following;
        private NavMeshAgent _agent;
        private Construction _currentCleaner;
        private BuildStorage _currentHouse;
        private BuildStorage _currentStorage;
        private BuildingsPull _pull;
        private Employee _employee;
        private Transform _target;

        private void Start() => Initialize();
        private void FixedUpdate() => Execute();

        private void Execute()
        {
            if (CanWalk()) Walk();
            else
            {
                _target = null;
                StopMove();
                StateExecute();
            }
        }

        private void Initialize()
        {
            _pull ??= FindObjectOfType<BuildingsPull>();
            _employee ??= GetComponent<Employee>();
            _agent = Instantiate(_navMeshPrefab, transform.position, Quaternion.identity);
            _agent.speed = _employee.GetWalkSpeed();
            _following = gameObject.AddComponent<Following>();
            _following.followObject = _agent.transform;
            _currentEmployeeState = EmployeeStates.Idle;
        }

        private void CheckConditions()
        {
            if (CanStartTransportation())
            {
                _currentEmployeeState = EmployeeStates.Transportation;
                SetTarget(_currentStorage.transform);
                return;
            }
            
            if (CanStartRecycle())
            {
                _currentEmployeeState = EmployeeStates.Recycling;
                SetTarget(_currentCleaner.transform);
                return;
            }

            if (CanStartPick())
            {
                _currentEmployeeState = EmployeeStates.Picking;
                SetTarget(_currentHouse.transform);
            }
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

        private void Picking()
        {
            if (CanStopPick()) _currentEmployeeState = EmployeeStates.Idle;
            print("�������...");
        }

        private void Recycling()
        {
            if (CanStopRecycle()) _currentEmployeeState = EmployeeStates.Idle;
            print("�����������...");
        }

        private void Transportation()
        {
            if (CanStopTransportation()) _currentEmployeeState = EmployeeStates.Idle;
            print("�������������...");
        }

        private bool CanWalk() => _target != null && !IsDestination();

        private bool CanStartTransportation() => CountCleanFluff() >= _fluffCapacity && HasStorage();

        private bool CanStartRecycle() => CountCleanFluff() < _fluffCapacity &&
                                          CountUncleanFluff() >= _fluffCapacity && HasCleaner();

        private bool CanStartPick() => CountUncleanFluff() < _fluffCapacity && HasGagaHouse();

        // ��������, ���� �� ���������� �� ���������
        private bool CanStopPick() => CountUncleanFluff() >= _fluffCapacity || !HasUncleanFluff() || !HasGagaHouse();

        // ������������, ���� ������ �� ���������� �� ��������� � ���� ���� �����������
        private bool CanStopRecycle() => CountCleanFluff() >= _fluffCapacity ||
                                     CountUncleanFluff() <= 0 ||
                                     !HasCleaner();

        // ����� � ������, ���� �� �������� ���� ��������� ���
        private bool CanStopTransportation() => CountCleanFluff() <= 0 || !HasStorage();
        private bool HasUncleanFluff() => _currentHouse.GetFluffCount() > 0;
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

        private void SetTarget(Transform target)
        {
            _target = target.transform;

            if (target.TryGetComponent(out Construction construction))
            {
                var entryPoint = construction.GetEntryPoint();
                if (entryPoint != null) _target = entryPoint;
            }

            _agent.SetDestination(_target.position);
            ChangeMindCloud(_currentEmployeeState);
        }

        private void Walk() => base.WalkAnimation(_target.position - transform.position);
        private bool IsDestination() => Vector2.Distance(transform.position, _target.position) <= _requireDistance;

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
        
        private bool HasGagaHouse()
        {
            var gagaHouses = _pull.GagaHouses;

            foreach (var house in gagaHouses)
            {
                var bMenu = house.GetBuildMenu();

                if (bMenu.IsBuilded &&
                    !house.IsOccupied() &&
                    bMenu.GetBuilding().TryGetComponent(out BuildStorage bStorage) &&
                    bStorage.GetFluffCount() > 0)
                {
                    _currentHouse = bStorage;
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