using System.Collections.Generic;
using UnityEngine;

namespace Characters.PathFinding.Algorithms
{
    public class DijkstraPathFind : AbstractPathFind
    {
        private Node _nodeToCheck;
        private Dictionary<int, Node> _visited = new();
        private Queue<Node> _toVisit = new();
        private Dictionary<int, Node> _toVisitDictionary = new();

        public DijkstraPathFind(
                Vector2 currentPos,
                Vector2 targetPos,
                LayerMask layer,
                float radius,
                string name) :
                base(
                        currentPos,
                        targetPos,
                        layer,
                        radius,
                        name)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            _visited = new Dictionary<int, Node>();
            _toVisit = new Queue<Node>();
            _toVisitDictionary = new Dictionary<int, Node>();

            _toVisit.Enqueue(_startNode);
            _toVisitDictionary.Add(_startNode.GetHashCode(), _startNode);

            isFinded = false;
            isWorked = true;
        }

        public override void Search()
        {
            if (_toVisit.Count <= 0) return;

            _nodeToCheck = _toVisit.Dequeue();
            _nodeToCheck = _toVisitDictionary[_nodeToCheck.GetHashCode()];
            _toVisitDictionary.Remove(_nodeToCheck.GetHashCode());

            if (!IsValidNode(_nodeToCheck.currentPosition)) return;

            _visited.Add(_nodeToCheck.GetHashCode(), _nodeToCheck);
            List<Node> neighbours = GetNeighbourNodes(_nodeToCheck);

            foreach (Node neighbour in neighbours)
            {
                if (!_visited.ContainsKey(neighbour.GetHashCode()) &&
                    !_toVisitDictionary.ContainsKey(neighbour.GetHashCode()))
                {
                    _toVisit.Enqueue(neighbour);
                    _toVisitDictionary.Add(neighbour.GetHashCode(), neighbour);
                }
                else if (_visited.ContainsKey(neighbour.GetHashCode())
                         && _visited[neighbour.GetHashCode()].distStartToNode > neighbour.distStartToNode)
                {
                    _visited.Remove(neighbour.GetHashCode());

                    _toVisit.Enqueue(neighbour);
                    _toVisitDictionary.Add(neighbour.GetHashCode(), neighbour);
                }
                else if (_toVisitDictionary.ContainsKey(neighbour.GetHashCode())
                         && _toVisitDictionary[neighbour.GetHashCode()].distStartToNode > neighbour.distStartToNode)
                {
                    _toVisitDictionary[neighbour.GetHashCode()] = neighbour;
                }
            }
        }

        public override void Draw()
        {
            foreach (var item in _toVisit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(new Vector2(item.currentPosition.x, item.currentPosition.y), _radius);
            }

            foreach (var item in _visited)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(new Vector2(item.Value.currentPosition.x, item.Value.currentPosition.y), _radius);
            }

            if (_nodeToCheck != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(new Vector2(_nodeToCheck.currentPosition.x, _nodeToCheck.currentPosition.y), _radius);
            }

            if (_path == null) return;
            
            foreach (var item in _path)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(new Vector2(item.x, item.y), _radius);
            }
        }
    }
}