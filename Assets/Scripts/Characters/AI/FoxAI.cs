using Characters.AI;
using UnityEngine;

namespace Charcters.AI
{
    public class FoxAI : PatrollerAI
    {
        [SerializeField] private float _requirePlayerDistance;
        [SerializeField] private float _requireFlagDistance;
        [SerializeField] private float _radius;
        
        private Transform _player;
        private Transform _flag;

        protected override void CheckConditions()
        {
            if (_player != null)
            {
                if (IsLessDistance(_player, _requirePlayerDistance) && CurrentState != EnemyStates.Run)
                {
                    CurrentDirection = GetOppositeDirection(_player.position, false);
                    CurrentState = EnemyStates.Run;
                }

                return;
            }

            if (IsLessFlags())
            {
                if (!IsLessDistance(_flag, _requireFlagDistance) || CurrentState == EnemyStates.Run) return;
                
                StopCoroutine(ChangeDirection(GetRandomDirection()));
                StartCoroutine(ChangeDirection(GetOppositeDirection(_flag.position, true)));
                CurrentState = EnemyStates.Run;

                return;
            }

            if (CurrentState == EnemyStates.Run) _canChangePatrolState = true;

            base.CheckConditions();
        }

        private bool IsLessDistance(Transform obj, float requireDistance) =>
                Vector2.Distance(transform.position, obj.position) < requireDistance;

        private bool IsLessFlags()
        {
            var cols = Physics2D.OverlapCircleAll(transform.position, _radius);
            
            foreach (var col in cols )
            {
                if (!col.TryGetComponent(out Flag flag) || !flag.isFlagAdded) continue;
                
                _flag = flag.transform;
                return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}