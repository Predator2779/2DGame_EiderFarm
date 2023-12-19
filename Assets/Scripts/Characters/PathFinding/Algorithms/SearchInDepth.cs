using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.PathFinding.Algorithms
{
    public class SearchInDepth : AbstractPathFind
    {
        private Node _nodeToCheck;
        private Dictionary<int, Node> _visited = new();
        private Stack<Node> _toVisit = new();

        public SearchInDepth(
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
        {
        }


        public override void Initialize()
        {
            base.Initialize();

            _visited = new Dictionary<int, Node>();
            _toVisit = new Stack<Node>();
            _toVisit.Push(_startNode);

            isFinded = false;
            isWorked = true;
        }

        public override void Search()
        {
            if (_toVisit.Count <= 0) return;

            _nodeToCheck = _toVisit.Pop();

            if (CheckDestination(_nodeToCheck.currentPosition))
            {
                _path = CalculatePathFromNode(_nodeToCheck);
                isFinded = true;
                isWorked = false;
            }

            if (!IsValidNode(_nodeToCheck.currentPosition))
            {
                _visited.Add(_nodeToCheck.GetHashCode(), _nodeToCheck);
                return;
            }

            if (_visited.All(x => x.Value.currentPosition != _nodeToCheck.currentPosition))
            {
                _visited.Add(_nodeToCheck.GetHashCode(), _nodeToCheck);

                List<Node> neighbours = GetNeighbourNodes(_nodeToCheck);

                foreach (Node neighbour in neighbours)
                    if (!_visited.ContainsKey(neighbour.GetHashCode()) && !_toVisit.Contains(neighbour))
                        _toVisit.Push(neighbour);
            }

            // switch (IsValidNode(_nodeToCheck.currentPosition))
            // {
            //     case true:
            //     {
            //         if (_visited.All(x => x.Value.currentPosition != _nodeToCheck.currentPosition))
            //         {
            //             _visited.Add(_nodeToCheck.GetHashCode(), _nodeToCheck);
            //
            //             neighbours = GetNeighbourNodes(_nodeToCheck);
            //
            //             foreach (Node neighbour in neighbours)
            //             {
            //                 if (!_visited.ContainsKey(neighbour.GetHashCode()) && !_toVisit.Contains(neighbour))
            //                 {
            //                     _toVisit.Push(neighbour);
            //                 }
            //             }
            //         }
            //
            //         break;
            //     }
            //     case false:
            //         _visited.Add(_nodeToCheck.GetHashCode(), _nodeToCheck);
            //         break;
            // }
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

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(new Vector2(_nodeToCheck.currentPosition.x, _nodeToCheck.currentPosition.y), _radius);

            if (_path == null) return;

            foreach (var item in _path)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(new Vector2(item.x, item.y), _radius);
            }
        }
    }
}