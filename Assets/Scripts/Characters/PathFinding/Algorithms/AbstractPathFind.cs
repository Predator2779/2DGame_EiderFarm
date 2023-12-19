using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.PathFinding.Algorithms
{
    public abstract class AbstractPathFind
    {
        public bool isFinded;
        public bool isWorked;

        private Vector2 _currentPos;
        private Vector2 _targetPos;
        private LayerMask _solidLayer;
        private float _requireDistance;
        
        protected float _radius;
        protected Node _startNode;
        protected List<Vector2> _path;

        public AbstractPathFind(
                Vector2 currentPos, 
                Vector2 targetPos,
                LayerMask layer, 
                float radius, 
                float requireDistance)
        {
            _currentPos = currentPos;
            _targetPos = targetPos;
            _solidLayer = layer;
            _radius = radius;
            _requireDistance = requireDistance;
        }

        public virtual void Initialize()
        {
            _startNode = new Node(0, _currentPos, _targetPos, null);
        }
        
        public abstract void Search();
        public List<Vector2> GetPath() => _path;
        public abstract void Draw();
        
        protected bool CheckDestination(Vector2 nodePosition) => 
                Vector2.Distance(nodePosition, _targetPos) <= _radius * 2;

        protected bool IsValidNode(Vector2 nodePosition)
        {
            var colliders = Physics2D.OverlapCircleAll(nodePosition, _radius, _solidLayer);

            return colliders.All(_col => !_col.CompareTag("Obstacle") && !_col.GetComponent<Person>());
        }
        protected List<Vector2> CalculatePathFromNode(Node node)
        {
            var path = new List<Vector2>();
            Node currentNode = node;

            while (currentNode.previousNode != null)
            {
                path.Add(new Vector2(currentNode.currentPosition.x, currentNode.currentPosition.y));
                currentNode = currentNode.previousNode;
            }

            return path;
        }
        protected List<Node> GetNeighbourNodes(Node node)
        {
            var neighbours = new List<Node>();

            float value = _radius * 2;
            
            neighbours.Add(new Node(node.distStartToNode + value, new Vector2(
                            node.currentPosition.x - value, node.currentPosition.y),
                    node.targetPosition,
                    node));
            neighbours.Add(new Node(node.distStartToNode + value, new Vector2(
                            node.currentPosition.x + value, node.currentPosition.y),
                    node.targetPosition,
                    node));
            neighbours.Add(new Node(node.distStartToNode + value, new Vector2(
                            node.currentPosition.x, node.currentPosition.y - value),
                    node.targetPosition,
                    node));
            neighbours.Add(new Node(node.distStartToNode + value, new Vector2(
                            node.currentPosition.x, node.currentPosition.y + value),
                    node.targetPosition,
                    node));

            return neighbours;
        }
    }
}