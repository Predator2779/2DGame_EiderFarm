using UnityEngine;

namespace Characters.PathFinding
{
    public class Node
    {
        public Vector2 currentPosition;
        public Vector2 targetPosition;
        public readonly Node previousNode;
        public readonly int distStartToNode;
        public readonly int distNodeToTarget;
        public readonly int distTotal;

        public Node(int distance, Vector2 nodePos, Vector2 targetPos, Node prevNode)
        {
            currentPosition = nodePos;
            targetPosition = targetPos;
            previousNode = prevNode;
            distStartToNode = distance;
            distNodeToTarget = (int)Mathf.Abs(targetPosition.x - currentPosition.x) +
                               (int)Mathf.Abs(targetPosition.y - currentPosition.y);
            distTotal = distStartToNode + distNodeToTarget;
        }
    }
}