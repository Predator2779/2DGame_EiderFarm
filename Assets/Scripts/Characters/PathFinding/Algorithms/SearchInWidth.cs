using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.PathFinding.Algorithms
{
    public class SearchInWidth : AbstractPathFind
    {
        private Dictionary<int, Node> _visitedQueue = new();
        private Queue<Node> _toVisitQueue = new Queue<Node>();

        public SearchInWidth(
                Vector2 currentPos, 
                Vector2 targetPos, 
                LayerMask layer, 
                float radius, 
                float requireDistance) :
                base(
                        currentPos,
                        targetPos,
                        layer, 
                        radius, 
                        requireDistance)
        { }

        
        public override void Initialize()
        {
            base.Initialize();
            
            _visitedQueue = new Dictionary<int, Node>();
            _toVisitQueue = new Queue<Node>();
            _toVisitQueue.Enqueue(_startNode);

            isFinded = false;
            isWorked = true;
        }

        public override void Search()
        {
            if (_toVisitQueue.Count <= 0) return;

            Node nodeToCheck = _toVisitQueue.Dequeue();

            if (CheckDestination(nodeToCheck.currentPosition))
            {
                _path = CalculatePathFromNode(nodeToCheck);
                isFinded = true;
                isWorked = false;
            }

            List<Node> neighbours;

            switch (IsValidNode(nodeToCheck.currentPosition))
            {
                case true:
                {
                    if (_visitedQueue.All(x => x.Value.currentPosition != nodeToCheck.currentPosition))
                    {
                        _visitedQueue.Add(nodeToCheck.GetHashCode(), nodeToCheck);

                        neighbours = GetNeighbourNodes(nodeToCheck);

                        foreach (Node neighbour in neighbours)
                        {
                            if (!_visitedQueue.ContainsKey(neighbour.GetHashCode()) &&
                                !_toVisitQueue.Contains(neighbour))
                            {
                                _toVisitQueue.Enqueue(neighbour);
                            }
                        }
                    }

                    break;
                }
                case false:
                    _visitedQueue.Add(nodeToCheck.GetHashCode(), nodeToCheck);
                    break;
            }
        }

        public override void Draw()
        {
            foreach (var item in _toVisitQueue)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(new Vector2(item.currentPosition.x, item.currentPosition.y), _radius);
            }

            foreach (var item in _visitedQueue)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(new Vector2(item.Value.currentPosition.x, item.Value.currentPosition.y), _radius);
            }

            if (_path != null)
            {
                foreach (var item in _path)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(new Vector2(item.x, item.y), _radius);
                }
            }
        }
    }
}