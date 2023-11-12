using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gaga : MonoBehaviour
{
    enum State
    {
        InHome,
        WalkToHome,
        WalkToNature
    }

    private State currentState;

    public Movement movement;

    [SerializeField] private GameObject _targetPosition;
    private Vector2 endPosition;
    [SerializeField] private float _speed;


    Gaga(GameObject target)
    {
        _targetPosition = target;
    }

    private void Start()
    {
        currentState = State.WalkToHome;
        movement = GetComponent<Movement>();
        endPosition = new Vector2(transform.position.x, transform.position.y);
    }

    private void Update()
    {
        UpdateState();
    }

    private void MoveTo(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - new Vector2(transform.position.x, transform.position.y);
        if (direction.magnitude < 0.5f)
        {
            SetState(State.InHome);
            return;
        }
        movement.Move(direction.normalized * _speed * Time.deltaTime);
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
    private void UpdateState()
    {
        switch (currentState)
        {
            case State.InHome:
                ProccessInHome();
                break;

            case State.WalkToHome:
                MoveTo(new Vector2(_targetPosition.transform.position.x, _targetPosition.transform.position.y));
                break;

            case State.WalkToNature:
                MoveTo(endPosition);
                break;
        }
    }

    public void ProccessInHome()
    {
        StartCoroutine(Process(2));
    }

    private IEnumerator Process(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        SetState(State.WalkToNature);
    }

}
