using System;
using System.Linq;
using System.Collections.Generic;
using Characters;
using UnityEngine;

public class PathFinder2 : MonoBehaviour
{
    private Vector2 _currentPos;
    private Vector2 _targetPos;
    private PFindAlgorithm _algorithm;
    private LayerMask _solidLayer;
    private float _radius;
    private float _requireDistance;

    // AStar
    private Node _startNode;
    private List<Node> _checkedNodes = new();
    private List<Node> _waitingNodes = new(); //to stack

    // Depth
    private Dictionary<int, Node> _visited = new();
    private Stack<Node> _toVisit = new();

    public List<Vector2> pathToTarget;
    public bool isFinded;
    public bool isWorked;

    public void Initialize(
            Vector2 currentPos,
            Vector2 targetPos,
            PFindAlgorithm algorithm,
            LayerMask layer,
            float radius,
            float requireDistance
    )
    {
        _currentPos = currentPos;
        _targetPos = targetPos;
        _algorithm = algorithm;
        _solidLayer = layer;
        _radius = radius;
        _requireDistance = requireDistance;

        if (_currentPos == _targetPos) return;

        _startNode = new Node(0, _currentPos, _targetPos, null);
        
        switch (_algorithm)
        {
            case PFindAlgorithm.AStar:
                InitAStar();
                break;
            case PFindAlgorithm.Depth:
                InitSearchInDepth();
                break;
            case PFindAlgorithm.Width:
                break;
            case PFindAlgorithm.Directed:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void InitAStar()
    {
        _checkedNodes.Add(_startNode);
        _waitingNodes.AddRange(GetNeighbourNodes(_startNode));

        isFinded = false;
        isWorked = true;
    }

    private void Update()
    {
        if (!isWorked) return;

        switch (_algorithm)
        {
            case PFindAlgorithm.AStar:
                AStar();
                break;
            case PFindAlgorithm.Depth:
                SearchInDepth();
                break;
            case PFindAlgorithm.Width:
                break;
            case PFindAlgorithm.Directed:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void AStar()
    {
        if (_waitingNodes.Count <= 0) return;

        Node nodeToCheck = _waitingNodes.FirstOrDefault(x => x.F == _waitingNodes.Min(y => y.F));

        if (CheckDestination(nodeToCheck.Position))
        {
            pathToTarget = CalculatePathFromNode(nodeToCheck);
            isFinded = true;
            isWorked = false;
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
                break;
            }
            case false:
                _waitingNodes.Remove(nodeToCheck);
                _checkedNodes.Add(nodeToCheck);
                break;
        }
    }

    private void InitSearchInDepth()
    {
        _visited = new Dictionary<int, Node>();
        _toVisit = new Stack<Node>();
        _toVisit.Push(_startNode);

        isFinded = false;
        isWorked = true;
    }

    private void SearchInDepth()
    {
        if (_toVisit.Count <= 0) return;
        
        Node nodeToCheck = _toVisit.Pop();
        
        if (CheckDestination(nodeToCheck.Position))
        {
            pathToTarget = CalculatePathFromNode(nodeToCheck);
            isFinded = true;
            isWorked = false;
        }

        List<Node> neighbours;
        
        switch (IsValidNode(nodeToCheck.Position))
        {
            case true:
            {
                if (_visited.All(x => x.Value.Position != nodeToCheck.Position))
                {
                    _visited.Add(nodeToCheck.GetHashCode(), nodeToCheck);
                    
                    neighbours = GetNeighbourNodes(nodeToCheck);
                
                    foreach (Node neighbour in neighbours)
                    {
                        if (!_visited.ContainsKey(neighbour.GetHashCode()) && !_toVisit.Contains(neighbour))
                        {
                            _toVisit.Push(neighbour);
                        }
                    }
                }
                break;
            }
            case false:
                _visited.Add(nodeToCheck.GetHashCode(), nodeToCheck);
                break;
        }
    }

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

        return colliders.All(_col => !_col.CompareTag("Obstacle")
                                     // && !_col.GetComponent<Person>()
                                     );
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
        
        foreach (var item in _toVisit)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(new Vector2(item.Position.x, item.Position.y), _radius);
        }
        
        foreach (var item in _visited)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector2(item.Value.Position.x, item.Value.Position.y), _radius);
        }
    }

    public enum PFindAlgorithm
    {
        AStar,
        Depth,
        Width,
        Directed
    }
}