using Player;
using UnityEngine;

[RequireComponent(typeof(IMovable))]
public class InputHandler : MonoBehaviour
{
    [SerializeField] private MyJoystick _joystick;

    [Header("Joystick sensetivity.")]
    [SerializeField, Range(0, 1)] private float _sensitivity;
    [SerializeField] private PersonAnimate _personAnimate;
    private IMovable _movable;

    private void Start() => _movable = GetComponent<IMovable>();

    private void Update()
    {
        Vector3 direction = new Vector2(_joystick.Horizontal, _joystick.Vertical);

        if (direction.magnitude > _sensitivity) _movable.Move(direction.normalized);
        
        _personAnimate.Walk(direction, 
                !(direction.y == 0 && direction.x == 0));
    }
}