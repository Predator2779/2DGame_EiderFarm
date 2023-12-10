using Player;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Gaga : MonoBehaviour
{
    private enum State
    {
        Idle,
        Walk,
    }

    [SerializeField] private Movement _movement;
    [SerializeField] private int _timeInHome;
    [SerializeField] private float _speed;

    private State _currentState;
    private Transform _centerPoint;
    private float _moveRadius;
    private SpriteRenderer _sprite;
    private bool _isCoroutineRunning;
    private Vector2 _targetPos;

    [SerializeField] private PersonAnimate _personAnimate;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _currentState = State.Walk;
        _movement = GetComponent<Movement>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _personAnimate = GetComponentInChildren<PersonAnimate>();
        _animator = GetComponentInChildren<Animator>();
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
        Vector2 direction = (_targetPos - new Vector2(transform.position.x,transform.position.y)).normalized;
        _movement.Move(direction.normalized * _speed * Time.deltaTime);
        _personAnimate.Walk(direction, true);
        float directionX = direction.x;
        _sprite.flipX = directionX < 0;

        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), _targetPos);
        if (distance < 3f)
        {
            _personAnimate.Walk(direction, false);
            SetState(State.Idle);
            GetRandomPoint(_moveRadius, _centerPoint);
        }
    }

    private void SetState(State newState)
    {
        switch (newState)
        {
            case State.Idle: _currentState = State.Idle; break;
            case State.Walk: _currentState = State.Walk; break;
        }
    }
    private void UpdateState(int timeInHome)
    {
        switch (_currentState)
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
        if (!_isCoroutineRunning)
        {
            _isCoroutineRunning = true;
            System.Random random = new System.Random();
            for (int i = 0; i < random.Next(0, maxIterations); i++)
            {
                _sprite.flipX = !_sprite.flipX;
                yield return new WaitForSecondsRealtime(random.Next(1, maxDelay));
            }
            SetState(State.Walk);
            _isCoroutineRunning = false;
        }
    }

    public void GetRandomPoint(float moveRadius, Transform center)
    {
        Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * moveRadius;

        _targetPos = new Vector2(center.position.x, center.position.y) + randomPoint;
    }
}
