using Characters.AI;
using UnityEngine;

namespace Charcters.AI
{
    public class FoxAI : PatrollerAI
    {
        [SerializeField] private float _requirePlayerDistance;
        [SerializeField] private float _requireFlagDistance;

        private Transform _player;
        private Transform _flag;

        protected override void CheckConditions()
        {
            if (_player != null)
            {
                if (IsLessDistance(_player, _requirePlayerDistance))
                {
                    CurrentDirection = GetOppositeDirection(_player.position, false);
                    CurrentState = EnemyStates.Run;
                    return;
                }
            }

            if (_flag != null)
            {
                if (IsLessDistance(_flag, _requireFlagDistance))
                {
                    StopCoroutine(ChangeDirection(GetRandomDirection()));
                    StartCoroutine(ChangeDirection(GetOppositeDirection(_flag.position, true)));
                    CurrentState = EnemyStates.Run;
                    return;
                }
            }

            base.CheckConditions();
        }

        private bool IsLessDistance(Transform obj, float requireDistance) =>
                Vector2.Distance(transform.position, obj.position) < requireDistance;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) _player = other.transform;

            if (other.TryGetComponent(out Flag flag) &&
                flag.isFlagAdded && _flag != flag)
                _flag = flag.transform;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player")) _player = null;

            if (other.TryGetComponent(out Flag flag) &&
                flag.isFlagAdded && _flag == flag)
                _flag = null;
        }
    }
}