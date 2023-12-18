using UnityEngine;

namespace Characters.PathFinding
{
    public class Node
    {
        public Vector2 currentPosition;
        public Vector2 targetPosition;
        public readonly Node previousNode;
        public readonly float distStartToNode;
        public readonly float distNodeToTarget;
        public readonly float distTotal;

        public Node(float distance, Vector2 nodePos, Vector2 targetPos, Node prevNode)
        {
            currentPosition = nodePos;
            targetPosition = targetPos;
            previousNode = prevNode;
            distStartToNode = distance;
            distNodeToTarget = Mathf.Abs(targetPosition.x - currentPosition.x) +
                               Mathf.Abs(targetPosition.y - currentPosition.y);
            distTotal = distStartToNode + distNodeToTarget;
        }
    }
}