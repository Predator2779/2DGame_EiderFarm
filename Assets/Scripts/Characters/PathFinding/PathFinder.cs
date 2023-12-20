using System;
using System.Collections.Generic;
using Characters.PathFinding.Algorithms;
using UnityEngine;

namespace Characters.PathFinding
{
    public class PathFinder : MonoBehaviour
    {
        [SerializeField] private int _findSpeed;
        [SerializeField] private LayerMask _solidLayer;

        private TypeFind _typeFind;
        private AbstractPathFind _algorithm;
        private Vector2 _currentPos;
        private Vector2 _targetPos;
        private float _radius;
        private string _name;

        public void Initialize(
                Vector2 currentPos,
                Vector2 targetPos,
                TypeFind type,
                float radius,
                string name
        )
        {
            _currentPos = currentPos;
            _targetPos = targetPos;
            _typeFind = type;
            _radius = radius;
            _name = name;

            if (_currentPos == _targetPos) return;

            switch (_typeFind)
            {
                case TypeFind.AStar:
                    _algorithm = new AStarPathFind(_currentPos, _targetPos, _solidLayer, _radius, _name);
                    break;
                case TypeFind.Depth:
                    _algorithm = new SearchInDepth(_currentPos, _targetPos, _solidLayer, _radius, _name);
                    break;
                case TypeFind.Width:
                    _algorithm = new SearchInWidth(_currentPos, _targetPos, _solidLayer, _radius, _name);
                    break;
                case TypeFind.Greedy:
                    _algorithm = new GreedyPathFind(_currentPos, _targetPos, _solidLayer, _radius, _name);
                    break;
                case TypeFind.Dijkstra:
                    _algorithm = new DijkstraPathFind(_currentPos, _targetPos, _solidLayer, _radius, _name);
                    break;
            }

            _algorithm.Initialize();
        }

        private void Update()
        {
            if (!IsWorked()) return;

            for (int i = 0; i < _findSpeed; i++)
                _algorithm.Search();
        }

        private void OnDrawGizmos()
        {
            if (_algorithm != null) _algorithm.Draw();
        }

        public bool IsWorked() => _algorithm != null && _algorithm.isWorked;

        public bool IsFinded() => _algorithm != null && _algorithm.isFinded;

        public List<Vector2> GetPath() => _algorithm?.GetPath();

        public void Deinitialize()
        {
            if (_algorithm == null) return;

            _algorithm.isFinded = false;
            _algorithm.isWorked = false;
            _algorithm = null;
        }
        
        public enum TypeFind
        {
            AStar,
            Depth,
            Width,
            Greedy,
            Dijkstra
        }
    }
}