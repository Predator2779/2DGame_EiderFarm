using System;
using System.Linq;
using System.Collections.Generic;
using Characters;
using Characters.PathFinding;
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

    // Width
    private Dictionary<int, Node> _visitedQueue = new();
    private Queue<Node> _toVisitQueue = new Queue<Node>();

    // Greedy
    Dictionary<int, Node> _visitedGreedy = new Dictionary<int, Node>();
    SortedSet<Node> _toVisitGreedy = new SortedSet<Node>(new CellComparer()); //
    Dictionary<int, Node> _toVisitDicGreedy = new Dictionary<int, Node>();

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
                InitSearchInWidth();
                break;
            case PFindAlgorithm.Greedy:
                InitGreedy();
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
                SearchAStar();
                break;
            case PFindAlgorithm.Depth:
                SearchInDepth();
                break;
            case PFindAlgorithm.Width:
                SearchInWidth();
                break;
            case PFindAlgorithm.Greedy:
                GreedySearch();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SearchAStar()
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

    private void InitSearchInWidth()
    {
        _visitedQueue = new Dictionary<int, Node>();
        _toVisitQueue = new Queue<Node>();
        _toVisitQueue.Enqueue(_startNode);

        isFinded = false;
        isWorked = true;
    }

    private void SearchInWidth()
    {
        if (_toVisitQueue.Count <= 0) return;

        Node nodeToCheck = _toVisitQueue.Dequeue();

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
                if (_visitedQueue.All(x => x.Value.Position != nodeToCheck.Position))
                {
                    _visitedQueue.Add(nodeToCheck.GetHashCode(), nodeToCheck);

                    neighbours = GetNeighbourNodes(nodeToCheck);

                    foreach (Node neighbour in neighbours)
                    {
                        if (!_visitedQueue.ContainsKey(neighbour.GetHashCode()) && !_toVisitQueue.Contains(neighbour))
                        {
                            _toVisitQueue.Enqueue(neighbour);
                        }
                    }
                }

                break;
            }
            case false:
                _visitedQueue.Add(nodeToCheck.GetHashCode(), nodeToCheck);
                break;
        }
    }

    private void InitGreedy()
    {
        _visitedGreedy = new Dictionary<int, Node>();
        _toVisitGreedy = new SortedSet<Node>(new CellComparer());
        _toVisitDicGreedy = new Dictionary<int, Node>();

        _toVisitGreedy.Add(_startNode);
        _toVisitDicGreedy.Add(_startNode.GetHashCode(), _startNode);
        
        isFinded = false;
        isWorked = true;
    }

    private void GreedySearch()
    {
        if (_toVisitGreedy.Count <= 0) return;
        
        Node nodeToCheck = _toVisitGreedy.Min;
        
        _visitedGreedy.Add(nodeToCheck.GetHashCode(), nodeToCheck);
        _toVisitGreedy.Remove(nodeToCheck);
        _toVisitDicGreedy.Remove(nodeToCheck.GetHashCode());

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
                neighbours = GetNeighbourNodes(nodeToCheck);
                foreach (Node neighbour in neighbours)
                {
                    if (!_visitedGreedy.ContainsKey(neighbour.GetHashCode()) && !_toVisitDicGreedy.ContainsKey(neighbour.GetHashCode()))
                    {
                        _toVisitGreedy.Add(neighbour);
                        _toVisitDicGreedy.Add(neighbour.GetHashCode(), neighbour);
                    }
                }
                break;
            }
            case false:
                _visitedGreedy.Add(nodeToCheck.GetHashCode(), nodeToCheck);
                break;
        }
    }
    
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
        foreach (var item in _waitingNodes)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector2(item.Position.x, item.Position.y), _radius);
        }

        foreach (var item in _checkedNodes)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(new Vector2(item.Position.x, item.Position.y), _radius);
        }

        foreach (var item in _toVisit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector2(item.Position.x, item.Position.y), _radius);
        }

        foreach (var item in _visited)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(new Vector2(item.Value.Position.x, item.Value.Position.y), _radius);
        }

        foreach (var item in _toVisitQueue)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector2(item.Position.x, item.Position.y), _radius);
        }

        foreach (var item in _visitedQueue)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(new Vector2(item.Value.Position.x, item.Value.Position.y), _radius);
        }

        foreach (var item in _visitedGreedy)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(new Vector2(item.Value.Position.x, item.Value.Position.y), _radius);
        }   
        
        foreach (var item in _toVisitDicGreedy)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector2(item.Value.Position.x, item.Value.Position.y), _radius);
        }
        
        if (pathToTarget != null)
        {
            foreach (var item in pathToTarget)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(new Vector2(item.x, item.y), _radius);
            }
        }
    }

    public enum PFindAlgorithm
    {
        AStar,
        Depth,
        Width,
        Greedy
    }
}