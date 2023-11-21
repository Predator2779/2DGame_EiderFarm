using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Gaga : MonoBehaviour
{
    public event Action GagaDieEvent;
    private enum State
    {
        Idle,
        Walk,
    }

    private State currentState;
    [SerializeField] private Field _field;
    [SerializeField] private Movement _movement;
    [SerializeField] private int _timeInHome;
    [SerializeField] private float _speed;

    private Transform _centerPoint;
    private float _moveRadius;

    private SpriteRenderer sprite;

    private bool isCoroutineRunning;

    private Vector2 targetPos;

    

    private void Start()
    {
        _field = FindObjectOfType<Field>();
        currentState = State.Walk;
        _movement = GetComponent<Movement>();
        sprite = GetComponent<SpriteRenderer>();
        
    }

    public void Initialize(Transform center, float radius)
    {
        _centerPoint = center;
        _moveRadius = radius;
    }

    private void Update()
    {
        UpdateState(_timeInHome);
    }

    private void MoveTo()
    {
        Vector2 direction = (targetPos - new Vector2(transform.position.x,transform.position.y)).normalized;
        _movement.Move(direction.normalized * _speed * Time.deltaTime);

        float directionX = direction.x;
        sprite.flipX = directionX < 0;

        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), targetPos);
        if (distance < 3f)
        {
            SetState(State.Idle);
            GetRandomPoint(_moveRadius, _centerPoint);
        }
    }

    private void SetState(State newState)
    {
        switch (newState)
        {
            case State.Idle: currentState = State.Idle; break;
            case State.Walk: currentState = State.Walk; break;
        }
    }
    private void UpdateState(int timeInHome)
    {
        switch (currentState)
        {
            case State.Idle:
                StartCoroutine(IdleProcess(4,3));
                break;

            case State.Walk:
                MoveTo();
                break;

        }
    }


    private IEnumerator IdleProcess(int maxIterations, int maxDelay)
    {
        if (!isCoroutineRunning)
        {
            isCoroutineRunning = true;
            System.Random random = new System.Random();
            for (int i = 0; i < random.Next(0, maxIterations); i++)
            {
                sprite.flipX = !sprite.flipX;
                yield return new WaitForSecondsRealtime(random.Next(1, maxDelay));
            }
            SetState(State.Walk);
            isCoroutineRunning = false;
        }
    }

    public void GetRandomPoint(float moveRadius, Transform center)
    {
        Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * moveRadius;

        targetPos = new Vector2(center.position.x, center.position.y) + randomPoint;

    }
}
