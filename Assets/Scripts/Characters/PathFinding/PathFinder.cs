using System;
using System.Linq;
using System.Collections.Generic;
using Characters;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [NonSerialized] public GameObject Target;
    public List<Vector2> PathToTarget;
    public List<Node> CheckedNodes = new();
    public List<Node> WaitingNodes = new();
    public LayerMask SolidLayer;
    public float radius;

    public List<Vector2> GetPath(Vector2 target)
    {
        PathToTarget = new List<Vector2>();
        CheckedNodes = new List<Node>();
        WaitingNodes = new List<Node>();

        Vector2 StartPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 TargetPosition = new Vector2(Mathf.Round(target.x), Mathf.Round(target.y));

        if (StartPosition == TargetPosition) return PathToTarget;

        Node startNode = new Node(0, StartPosition, TargetPosition, null);

        CheckedNodes.Add(startNode);
        WaitingNodes.AddRange(GetNeighbourNodes(startNode));

        while (WaitingNodes.Count > 0)
        {
            Node nodeToCheck = WaitingNodes.FirstOrDefault(x => x.F == WaitingNodes.Min(y => y.F));

            if (nodeToCheck.Position == TargetPosition)
            {
                return CalculatePathFromNode(nodeToCheck);
            }

            var isValid = IsValidNode(nodeToCheck.Position);

            switch (isValid)
            {
                case false:
                    WaitingNodes.Remove(nodeToCheck);
                    CheckedNodes.Add(nodeToCheck);
                    break;
                case true:
                {
                    WaitingNodes.Remove(nodeToCheck);
                    if (CheckedNodes.All(x => x.Position != nodeToCheck.Position))
                    {
                        CheckedNodes.Add(nodeToCheck);
                        WaitingNodes.AddRange(GetNeighbourNodes(nodeToCheck));
                    }

                    break;
                }
            }
        }

        return PathToTarget;
    }

    private bool IsValidNode(Vector2 nodePosition)
    {
        var colliders = Physics2D.OverlapCircleAll(nodePosition, radius, SolidLayer);

        foreach (var _col in colliders)
        {
            if (_col.CompareTag("Obstacle") || _col.GetComponent<Person>()) return false;
        }

        return true;
    }

    private List<Vector2> CalculatePathFromNode(Node node)
    {
        var path = new List<Vector2>();
        Node currentNode = node;

        while (currentNode.PreviousNode != null)
        {
            path.Add(new Vector2(currentNode.Position.x, currentNode.Position.y));
            currentNode = currentNode.PreviousNode;
        }

        return path;
    }

    private List<Node> GetNeighbourNodes(Node node)
    {
        var Neighbours = new List<Node>();

        Neighbours.Add(new Node(node.G + 1, new Vector2(
                        node.Position.x - 1, node.Position.y),
                node.TargetPosition,
                node));
        Neighbours.Add(new Node(node.G + 1, new Vector2(
                        node.Position.x + 1, node.Position.y),
                node.TargetPosition,
                node));
        Neighbours.Add(new Node(node.G + 1, new Vector2(
                        node.Position.x, node.Position.y - 1),
                node.TargetPosition,
                node));
        Neighbours.Add(new Node(node.G + 1, new Vector2(
                        node.Position.x, node.Position.y + 1),
                node.TargetPosition,
                node));
        return Neighbours;
    }

    // private void OnDrawGizmos()
    // {
    //     if (PathToTarget == null)
    //     {
    //         foreach (var item in PathToTarget)
    //         {
    //             Gizmos.color = Color.red;
    //             Gizmos.DrawSphere(new Vector2(item.x, item.y), radius);
    //         }
    //     }
    //
    //     foreach (var item in CheckedNodes)
    //     {
    //         Gizmos.color = Color.blue;
    //         Gizmos.DrawSphere(new Vector2(item.Position.x, item.Position.y), radius);
    //     }
    // }
}

public class Node
{
    public Vector2 Position;
    public Vector2 TargetPosition;
    public Node PreviousNode;
    public int G;
    public int H;
    public int F;

    public Node(int g, Vector2 nodePosition, Vector2 targetPosition, Node previousNode)
    {
        Position = nodePosition;
        TargetPosition = targetPosition;
        PreviousNode = previousNode;
        G = g;
        H = (int)Mathf.Abs(targetPosition.x - Position.x) + (int)Mathf.Abs(targetPosition.y - Position.y);
        F = G + H;
    }
}