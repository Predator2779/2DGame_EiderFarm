using System.Collections.Generic;
using Characters.PathFinding.Nodes;
using UnityEngine;

namespace Characters.PathFinding.Algorithms
{
    public class GreedyPathFind : AbstractPathFind
    {
        private Node _nodeToCheck;
        private Dictionary<int, Node> _visitedGreedy = new();
        private SortedSet<Node> _toVisitGreedy = new(new NodeComparer());
        private Dictionary<int, Node> _toVisitDicGreedy = new();

        public GreedyPathFind(
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

            _visitedGreedy = new Dictionary<int, Node>();
            _toVisitGreedy = new SortedSet<Node>(new NodeComparer());
            _toVisitDicGreedy = new Dictionary<int, Node>();

            _toVisitGreedy.Add(_startNode);
            _toVisitDicGreedy.Add(_startNode.GetHashCode(), _startNode);

            isFinded = false;
            isWorked = true;
        }

        public override void Search()
        {
            if (_toVisitGreedy.Count <= 0) return;

            _nodeToCheck = _toVisitGreedy.Min;

            _visitedGreedy.Add(_nodeToCheck.GetHashCode(), _nodeToCheck);
            _toVisitGreedy.Remove(_nodeToCheck);
            _toVisitDicGreedy.Remove(_nodeToCheck.GetHashCode());

            if (CheckDestination(_nodeToCheck.currentPosition))
            {
                _path = CalculatePathFromNode(_nodeToCheck);
                isFinded = true;
                isWorked = false;
            }

            if (!IsValidNode(_nodeToCheck.currentPosition)) return;

            List<Node> neighbours = GetNeighbourNodes(_nodeToCheck);

            foreach (Node neighbour in neighbours)
            {
                if (!_visitedGreedy.ContainsKey(neighbour.GetHashCode()) &&
                    !_toVisitDicGreedy.ContainsKey(neighbour.GetHashCode()))
                {
                    _toVisitGreedy.Add(neighbour);
                    _toVisitDicGreedy.Add(neighbour.GetHashCode(), neighbour);
                }
            }
        }

        public override void Draw()
        {
            foreach (var item in _visitedGreedy)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(new Vector2(item.Value.currentPosition.x, item.Value.currentPosition.y), _radius);
            }

            foreach (var item in _toVisitDicGreedy)
            {
                Gizmos.color = Color.red;
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