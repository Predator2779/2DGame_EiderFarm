using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.PathFinding.Algorithms
{
    public class AStarPathFind : AbstractPathFind
    {
        private Node _nodeToCheck;
        private List<Node> _checkedNodes = new();
        private List<Node> _waitingNodes = new(); //to stack

        public AStarPathFind(
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

            _checkedNodes = new();
            _waitingNodes = new();
            
            _checkedNodes.Add(_startNode);
            _waitingNodes.AddRange(GetNeighbourNodes(_startNode));

            isFinded = false;
            isWorked = true;
        }

        public override void Search()
        {
            if (_waitingNodes.Count <= 0) return;

            _nodeToCheck = _waitingNodes.FirstOrDefault(x => x.distTotal == _waitingNodes.Min(y => y.distTotal));

            if (CheckDestination(_nodeToCheck.currentPosition))
            {
                _path = CalculatePathFromNode(_nodeToCheck);
                isFinded = true;
                isWorked = false;
            }

            if (!IsValidNode(_nodeToCheck.currentPosition))
            {
                _waitingNodes.Remove(_nodeToCheck);
                _checkedNodes.Add(_nodeToCheck);
                return;
            }

            _waitingNodes.Remove(_nodeToCheck);

            if (_checkedNodes.All(x => x.currentPosition != _nodeToCheck.currentPosition))
            {
                _checkedNodes.Add(_nodeToCheck);
                _waitingNodes.AddRange(GetNeighbourNodes(_nodeToCheck));
            }
        }

        public override void Draw()
        {
            foreach (var item in _waitingNodes)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(new Vector2(item.currentPosition.x, item.currentPosition.y), _radius);
            }

            foreach (var item in _checkedNodes)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(new Vector2(item.currentPosition.x, item.currentPosition.y), _radius);
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