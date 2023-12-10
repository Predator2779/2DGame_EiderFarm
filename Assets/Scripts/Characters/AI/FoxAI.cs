using Characters;
using Characters.AI;
using UnityEngine;

namespace Charcters.AI
{
    public class FoxAI : PatrollerAI
    {
        [SerializeField] private float _requirePlayerDistance;
        [SerializeField] private float _requireFlagDistance;
        [SerializeField] private float _radius;
        
        private Transform _person;
        private Transform _flag;

        protected override void CheckConditions()
        {
            if (IsLessPerson())
            {
                if (!IsLessDistance(_person, _requirePlayerDistance) || CurrentState == EnemyStates.Run) return;
                
                CurrentDirection = GetOppositeDirection(_person.position, false);
                CurrentState = EnemyStates.Run;

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

        private bool IsLessPerson()
        {
            var cols = Physics2D.OverlapCircleAll(transform.position, _radius);
            
            foreach (var col in cols )
            {
                if (!col.TryGetComponent(out Person per)) continue;
                
                _person = per.transform;
                return true;
            }

            return false;
        }
        
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