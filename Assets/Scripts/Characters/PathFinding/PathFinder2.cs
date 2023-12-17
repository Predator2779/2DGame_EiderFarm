using System.Linq;
using System.Collections.Generic;
using Characters;
using UnityEngine;

public class PathFinder2 : MonoBehaviour
{
    private Vector2 _currentPos;
    private Vector2 _targetPos;
    private LayerMask _solidLayer;
    private float _radius;
    private float _requireDistance;

    private Node _startNode;
    private List<Node> _checkedNodes = new();
    private List<Node> _waitingNodes = new(); //to stack

    public List<Vector2> pathToTarget;
    public bool isFinded;
    public bool isWorked;

    public void Initialize(
            Vector2 currentPos,
            Vector2 targetPos,
            LayerMask layer,
            float radius,
            float requireDistance
    )
    {
        _currentPos = currentPos;
        _targetPos = targetPos;
        _solidLayer = layer;
        _radius = radius;
        _requireDistance = requireDistance;

        if (_currentPos == _targetPos) return;

        _startNode = new Node(0, _currentPos, _targetPos, null);
        _checkedNodes.Add(_startNode);
        _waitingNodes.AddRange(GetNeighbourNodes(_startNode));

        isFinded = false;
        isWorked = true;
    }

    private void Update()
    {
        if (isWorked) AStar();
    }

    private void AStar()
    {
        print("AStar...");

        if (_waitingNodes.Count <= 0) return;

        print("worked...");

        Node nodeToCheck = _waitingNodes.FirstOrDefault(x => x.F == _waitingNodes.Min(y => y.F));

        if (CheckDestination(nodeToCheck.Position))
        {
            pathToTarget = CalculatePathFromNode(nodeToCheck);
            isFinded = true;
            isWorked = false;

            print("finded.");
        }

        switch (IsValidNode(nodeToCheck.Position))
        {
            case true:
            {
                _waitingNodes.Remove(nodeToCheck);

                if (_checkedNodes.All(x => x.Position != nodeToCheck.Position))
                {
                    _checkedNodes.Add(nodeToCheck);
                    _waitingNodes.AddRange(GetNeighbourNodes(nodeToCheck));
                }

                print(true);
                break;
            }
            case false:
                _waitingNodes.Remove(nodeToCheck);
                _checkedNodes.Add(nodeToCheck);
                print(false);
                break;
        }
    }

    // public List<Vector2> SearchInDepth(Node entry)
    // {
    //     Dictionary<int, Node> visited = new Dictionary<int, Node>();
    //     Stack<Node> toVisit = new Stack<Node>();
    //
    //     toVisit.Push(entry);
    //
    //     while (toVisit.Count > 0)
    //     {
    //         Node current = toVisit.Pop();
    //
    //         if (current.Position == _targetPos)
    //         {
    //             return CalculatePathFromNode(current);
    //             // return current;
    //         }
    //
    //         visited.Add(current.GetHashCode(), current);
    //         List<Node> neighbours = GetNeighbourNodes(current);
    //
    //         foreach (Node neighbour in neighbours)
    //         {
    //             if (!visited.ContainsKey(neighbour.GetHashCode()) && !toVisit.Contains(neighbour))
    //             {
    //                 toVisit.Push(neighbour);
    //             }
    //         }
    //     }
    //
    //     return null;
    // }

    // private Node SearchInWidth(Node entry, Node target)
    // {
    //     Dictionary<int, Node> visited = new Dictionary<int, Node>();
    //     Queue<Node> toVisit = new Queue<Node>();
    //
    //     toVisit.Enqueue(entry);
    //
    //     while (toVisit.Count > 0)
    //     {
    //         Node current = toVisit.Dequeue();
    //
    //         if (current.Equals(target))
    //         {
    //             return current;
    //         }
    //
    //         visited.Add(current.GetHashCode(), current);
    //         List<Node> neighbours = GetNeighbourNodes(current);
    //         foreach (Node neighbour in neighbours)
    //         {
    //             if (!visited.ContainsKey(neighbour.GetHashCode()) && !toVisit.Contains(neighbour))
    //             {
    //                 toVisit.Enqueue(neighbour); ;
    //             }
    //         }
    //     }
    //
    //     return null;
    // }

    // private Node SearchDirected(Node entry, Node target)
    // {
    //     Dictionary<int, Node> visited = new Dictionary<int, Node>();
    //     SortedSet<Node> toVisit = new SortedSet<Node>(new CellComparer());
    //     Dictionary<int, Node> toVisitDic = new Dictionary<int, Node>();
    //     
    //     toVisit.Add(entry);
    //     toVisitDic.Add(entry.GetHashCode(), entry);
    //
    //     while (toVisit.Count > 0)
    //     {
    //         Node current = toVisit.Min;
    //         visited.Add(current.GetHashCode(), current);
    //         toVisit.Remove(current);
    //         toVisitDic.Remove(current.GetHashCode());
    //
    //         if (current.Equals(target))
    //         {
    //             return current;
    //         }
    //         List<Node> neighbours = GetNeighbourNodes(current);
    //         foreach (Node neighbour in neighbours)
    //         {
    //             if (!visited.ContainsKey(neighbour.GetHashCode()) && !toVisitDic.ContainsKey(neighbour.GetHashCode()))
    //             {
    //                 toVisit.Add(neighbour);
    //                 toVisitDic.Add(neighbour.GetHashCode(), neighbour);
    //             }
    //         }
    //     }
    //
    //     return null;
    // }

    private bool CheckDestination(Vector2 nodePosition)
    {
        float distance = Vector2.Distance(nodePosition, _targetPos);
        
        if (distance <= _requireDistance) return true;

        return false;
    }
    
    private bool IsValidNode(Vector2 nodePosition)
    {
        var colliders = Physics2D.OverlapCircleAll(nodePosition, _radius, _solidLayer);

        return colliders.All(_col => !_col.CompareTag("Obstacle") && !_col.GetComponent<Person>());
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

    private void OnDrawGizmos()
    {
        if (pathToTarget != null)
        {
            foreach (var item in pathToTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(new Vector2(item.x, item.y), _radius);
            }
        }

        foreach (var item in _checkedNodes)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(new Vector2(item.Position.x, item.Position.y), _radius);
        }
    }
}