using System;
using System.Collections;
using UnityEngine;

public class Gaga : MonoBehaviour
{
    public event Action GagaDieEvent;
    private enum State
    {
        InHome,
        WalkToHome,
        WalkToNature
    }

    private State currentState;

    [SerializeField] private Movement _movement;
    [SerializeField] private FluffGiver _fluffGiver;

    private GameObject targetPosition;
    private GameObject endPosition;
    [SerializeField] private float _speed;

    private SpriteRenderer sprite;

    [Header("����� ���������� ���� � ������ (����� ��������� ����).")]
    [SerializeField] private int _timeInHome;

    public void Initialize(GameObject target, GameObject endOfField)
    {
        targetPosition = target;
        endPosition = endOfField;
    }

    private void Start()
    {
        currentState = State.WalkToHome;
        _movement = GetComponent<Movement>();
        sprite = GetComponent<SpriteRenderer>();
        _fluffGiver = GetComponent<FluffGiver>();
    }

    private void Update()
    {
        UpdateState(_timeInHome);
    }

    private void MoveTo(GameObject target)
    {
        Vector2 direction = new Vector2(
                target.transform.position.x, target.transform.position.y) - 
                            new Vector2(transform.position.x, transform.position.y);
        
        if (direction.magnitude < 0.5f)
        {
            SetState(State.InHome);
            return;
        }
        _movement.Move(direction.normalized * _speed * Time.deltaTime);
    }

    private void IsEnd()
    {
        float distance = Vector2.Distance(
                new Vector2(transform.position.x, transform.position.y), 
                new Vector2(endPosition.transform.position.x, endPosition.transform.position.y));
        
        if (distance < 1f)
        {
            GagaDieEvent.Invoke();
            Destroy(gameObject);
        }
    }
    private void SetState(State newState)
    {
        switch (newState)
        {
            case State.InHome: currentState = State.InHome; break;
            case State.WalkToHome: currentState = State.WalkToHome; break;
            case State.WalkToNature: currentState = State.WalkToNature; break;
        }
    }
    private void UpdateState(int timeInHome)
    {
        switch (currentState)
        {
            case State.InHome:
                ProccessInHome(timeInHome);
                break;

            case State.WalkToHome:
                MoveTo(targetPosition);
                break;

            case State.WalkToNature:
                MoveTo(endPosition);
                IsEnd();
                break;
        }
    }

    public void ProccessInHome(float time)
    {
        StartCoroutine(Process(time));
    }

    private IEnumerator Process(float time)
    {
        sprite.enabled = false;

        yield return new WaitForSecondsRealtime(time);
        _fluffGiver.GiveFluff();
        SetState(State.WalkToNature);

        sprite.enabled = true;
    }

    public void GoToEnd()
    {
        SetState(State.WalkToNature);
    }

}
