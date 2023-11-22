using Player;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Person : MonoBehaviour, IMovable
{
    [Header("Character speed.")]
    [SerializeField, Range(0, 10)] private float _speed;

    private Movement _movement;

    private void OnValidate() => SetNullableFields();
    private void Start() => SetNullableFields();

    private void SetNullableFields()
    {
        _movement ??= GetComponent<Movement>();
    }

    public void Move(Vector2 direction)
    {
        _movement.Move(direction * _speed * Time.deltaTime);
    }
}