using System.Collections.Generic;
using Characters.PathFinding.Nodes;
using UnityEngine;

namespace Characters.PathFinding.Algorithms
{
    public class GreedyPathFind : AbstractPathFind
    {
        Dictionary<int, Node> _visitedGreedy = new();
        SortedSet<Node> _toVisitGreedy = new(new NodeComparer());
        Dictionary<int, Node> _toVisitDicGreedy = new();

        public GreedyPathFind(
                Vector2 currentPos,
                Vector2 targetPos,
                LayerMask layer,
                float radius) :
                base(
                        currentPos,
                        targetPos,
                        layer,
                        radius)
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

            Node nodeToCheck = _toVisitGreedy.Min;

            _visitedGreedy.Add(nodeToCheck.GetHashCode(), nodeToCheck);
            _toVisitGreedy.Remove(nodeToCheck);
            _toVisitDicGreedy.Remove(nodeToCheck.GetHashCode());

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
                    neighbours = GetNeighbourNodes(nodeToCheck);

                    foreach (Node neighbour in neighbours)
                    {
                        if (_visitedGreedy.ContainsKey(neighbour.GetHashCode()) ||
                            _toVisitDicGreedy.ContainsKey(neighbour.GetHashCode())) continue;

                        _toVisitGreedy.Add(neighbour);
                        _toVisitDicGreedy.Add(neighbour.GetHashCode(), neighbour);
                    }

                    break;
                }
                case false:
                    if (!_visitedGreedy.ContainsKey(nodeToCheck.GetHashCode()))
                        _visitedGreedy.Add(nodeToCheck.GetHashCode(), nodeToCheck);
                    break;
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