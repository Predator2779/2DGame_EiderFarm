using Player;
using System.Collections;
using UnityEngine;

public class Gaga : MonoBehaviour
{
    private enum State
    {
        Idle,
        Walk,
    }

    [SerializeField] private PersonAnimate _personAnimate;
    [SerializeField] private Animator _animator;
    [SerializeField] private Movement _movement;
    [SerializeField] private int _timeInHome;
    [SerializeField] private float _speed;

    private State _currentState;
    private Transform _centerPoint;
    private float _moveRadius;
    private SpriteRenderer _sprite;
    private bool _isCoroutineRunning;
    private Vector2 _targetPos;

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
        
        _personAnimate.Walk(direction, true, false);
        _movement.Move(direction.normalized * _speed * Time.deltaTime);
        _sprite.flipX = direction.x < 0;

        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), _targetPos);
        if (distance < 3f)
        {
            _personAnimate.Walk(direction, false, false);
            
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

    private void GetRandomPoint(float moveRadius, Transform center)
    {
        Vector2 randomPoint = Random.insideUnitCircle * moveRadius;

        _targetPos = new Vector2(center.position.x, center.position.y) + randomPoint;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        StopCoroutine(IdleProcess(4, 3));
        _targetPos *= -1;
        SetState(State.Walk);
    }
}
