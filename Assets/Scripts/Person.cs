using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Person : MonoBehaviour, IMovable
{
    [SerializeField] private Movement _movement;

    [Header("Character speed.")]
    [SerializeField, Range(0, 100)] private float _speed;

    private void Start() => _movement = GetComponent<Movement>();

    public void Move(Vector3 direction) => _movement.Move(direction * _speed * Time.deltaTime);
}