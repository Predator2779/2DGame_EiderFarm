using System;
using System.Collections.Generic;
using Characters.PathFinding.Algorithms;
using UnityEngine;

namespace Characters.PathFinding
{
    public class PathFinder : MonoBehaviour
    {
        private TypeFind _typeFind;
        private AbstractPathFind _algorithm;
        private Vector2 _currentPos;
        private Vector2 _targetPos;

        [SerializeField] private LayerMask _solidLayer;
        [SerializeField] private float _radius;
        [SerializeField] private float _requireDistance;

        public void Initialize(
                Vector2 currentPos,
                Vector2 targetPos,
                TypeFind type
        )
        {
            _currentPos = currentPos;
            _targetPos = targetPos;
            _typeFind = type;

            if (_currentPos == _targetPos) return;

            switch (_typeFind)
            {
                case TypeFind.AStar:
                    _algorithm = new AStarPathFind(_currentPos, _targetPos, _solidLayer, _radius, _requireDistance);
                    break;
                case TypeFind.Depth:
                    _algorithm = new SearchInDepth(_currentPos, _targetPos, _solidLayer, _radius, _requireDistance);
                    break;
                case TypeFind.Width:
                    _algorithm = new SearchInWidth(_currentPos, _targetPos, _solidLayer, _radius, _requireDistance);
                    break;
                case TypeFind.Greedy:
                    _algorithm = new GreedyPathFind(_currentPos, _targetPos, _solidLayer, _radius, _requireDistance);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_typeFind));
            }

            _algorithm.Initialize();
        }

        private void Update()
        {
            if (IsWorked()) _algorithm.Search();
        }

        private void OnDrawGizmos()
        {
            if (IsWorked()) _algorithm.Draw();
        }

        public bool IsWorked() => _algorithm != null && _algorithm.isWorked;

        public bool IsFinded() => _algorithm != null && _algorithm.isFinded;

        public List<Vector2> GetPath() => _algorithm?.GetPath();

        public enum TypeFind
        {
            AStar,
            Depth,
            Width,
            Greedy
        }
    }
}