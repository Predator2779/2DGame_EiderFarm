using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.PathFinding.Algorithms
{
    public class AStarPathFind : AbstractPathFind
    {
        private List<Node> _checkedNodes = new();
        private List<Node> _waitingNodes = new(); //to stack

        public AStarPathFind(
                Vector2 currentPos, 
                Vector2 targetPos, 
                LayerMask layer, 
                float radius) :
                base(
                        currentPos,
                        targetPos,
                        layer, 
                        radius) { }

        
        public override void Initialize()
        {
            base.Initialize();
            
            _checkedNodes.Add(_startNode);
            _waitingNodes.AddRange(GetNeighbourNodes(_startNode));

            isFinded = false;
            isWorked = true;
        }

        public override void Search()
        {
            if (_waitingNodes.Count <= 0) return;

            Node nodeToCheck = _waitingNodes.FirstOrDefault(x => x.distTotal == _waitingNodes.Min(y => y.distTotal));

            if (CheckDestination(nodeToCheck.currentPosition))
            {
                _path = CalculatePathFromNode(nodeToCheck);
                isFinded = true;
                isWorked = false;
            }

            switch (IsValidNode(nodeToCheck.currentPosition))
            {
                case true:
                {
                    _waitingNodes.Remove(nodeToCheck);

                    if (_checkedNodes.All(x => x.currentPosition != nodeToCheck.currentPosition))
                    {
                        _checkedNodes.Add(nodeToCheck);
                        _waitingNodes.AddRange(GetNeighbourNodes(nodeToCheck));
                    }

                    break;
                }
                case false:
                    _waitingNodes.Remove(nodeToCheck);
                    _checkedNodes.Add(nodeToCheck);
                    break;
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

            if (_path == null) return;

            foreach (var item in _path)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(new Vector2(item.x, item.y), _radius);
            }
        }
    }
}