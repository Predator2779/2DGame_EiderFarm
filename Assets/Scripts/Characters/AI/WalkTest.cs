using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI
{
    [RequireComponent(typeof(PathFinder))]
    public class WalkTest : WalkerAI
    {
        [SerializeField] private Transform _target;
        private PathFinder _pathFinder;
        private List<Vector2> _path = new();
        [SerializeField] private int _index;
        [SerializeField] private bool _walk;

        private void Start()
        {
            _pathFinder ??= GetComponent<PathFinder>();
        }

        private void Update()
        {
            if (_target != null) WalkToTarget();
        }

        private void WalkToTarget()
        {
            if (_walk)
            {
                if (!IsDestination(transform.position, _target.position))
                {
                    if (_index > 0)
                    {
                        if (_path == null) return;

                        if (!IsDestination(transform.position, _path[_index]))
                        {
                            var direction = _path[_index] - (Vector2)transform.position;
                            Walk(direction);
                        }
                        else
                        {
                            _index--;
                        }
                    }
                    else SetPath(_target.position);
                }
            }
            else
            {
                Idle();
            }
        }

        private void SetPath(Vector2 target)
        {
            _path = _pathFinder.GetPath(target);
            _index = _path.Count - 1;
        }

        private bool IsDestination(Vector2 first, Vector2 second) => (int)Vector2.Distance(first, second) == 0;

        private void Idle()
        {
            print("Idle");
            _personAnimate.Walk(_target.position, false);
        }
    }
}