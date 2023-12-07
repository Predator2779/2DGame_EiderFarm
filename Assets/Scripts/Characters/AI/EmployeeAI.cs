using System.Collections.Generic;
using System.Linq;
using Building;
using Characters;
using Characters.AI;
using Economy;
using General;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Characters.Enemy
{
    [RequireComponent(typeof(Employee))]
    [RequireComponent(typeof(PathFinder))]
    public class EmployeeAI : PatrollerAI
    {
        [Header("Service")]
        [SerializeField] private EmployeeStates _currentEmployeeState;

        [SerializeField] private BuildingsPull _pull;

        [FormerlySerializedAs("_uncleanFluffCapacity")]
        [Space] [Header("Settings:")]
        [SerializeField] [Range(1, 100)] private int _fluffCapacity;

        private PathFinder _pathFinder;
        private Employee _employee;
        private Vector2 _target;
        private List<Vector2> _path = new();
        private int _index;

        protected override void Initialize()
        {
            _employee ??= GetComponent<Employee>();
            _pathFinder ??= GetComponent<PathFinder>();

            SetPath(_target);
        }

        protected override void CheckConditions()
        {
            /// base.StateExecute
        }

        protected override void StateExecute()
        {
            WalkToTarget();

            // switch (_currentEmployeeState)
            // {
            //     case EmployeeStates.Idle:
            //         Idle();
            //         break;
            //     case EmployeeStates.Picking:
            //         Picking();
            //         break;
            //     case EmployeeStates.Recycling:
            //         Recycling();
            //         break;
            //     case EmployeeStates.Transportation:
            //         Transportation();
            //         break;
            // }
        }

        private void WalkToTarget()
        {
            if (CurrentState != EnemyStates.Run) return;

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

        private bool IsEnoughFluff()
        {
            var bunch = TryGetFluffBunch();
            return bunch != null && bunch.GetCount() >= _fluffCapacity;
        }

        private bool IsEnoughStorage()
        {
            return false;
        }
        
        private ItemBunch TryGetFluffBunch() => _employee.GetInventory().TryGetBunch(
                GlobalConstants.UncleanedFluff, out ItemBunch bunch) ? bunch : null;

        private void Picking()
        {
            throw new System.NotImplementedException();
        }

        private void Recycling()
        {
            throw new System.NotImplementedException();
        }

        private void Transportation()
        {
            throw new System.NotImplementedException();
        }

        private enum EmployeeStates
        {
            Idle,
            Picking,
            Recycling,
            Transportation
        }
    }
}