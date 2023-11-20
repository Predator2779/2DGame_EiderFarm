using System;
using Player;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Person : MonoBehaviour, IMovable
{
    [Header("Character speed.")]
    [SerializeField, Range(0, 10)] private float _speed;

    [SerializeField] private SpriteRenderer _renderer;

    private Movement _movement;
    private TurnHandler _turnHandler;

    private void OnValidate() => SetNullableFields();
    private void Start() => SetNullableFields();

    private void SetNullableFields()
    {
        _movement ??= GetComponent<Movement>();
        _turnHandler ??= new TurnHandler(_renderer);
    }
    
    public void Move(Vector2 direction)
    {
        _movement.Move(direction * _speed * Time.deltaTime);

        CheckSide(direction);
    }

    private void CheckSide(Vector2 direction)
    {
        if (direction.x > 0) _turnHandler.ChangeSide(TurnHandler.PlayerSides.Right);
        if (direction.x < 0) _turnHandler.ChangeSide(TurnHandler.PlayerSides.Left);
    }
}