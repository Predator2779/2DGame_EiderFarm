using UnityEngine;

[RequireComponent(typeof(IMovable))]
public class InputHandler : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;

    [Header("Joystick sensetivity.")]
    [SerializeField, Range(0, 1)] private float _sensitivity;

    private IMovable _movable;

    private void Start() => _movable = GetComponent<IMovable>();

    private void Update()
    {
        Vector3 direction = new Vector2(_joystick.Horizontal, _joystick.Vertical);

        if (direction.magnitude > _sensitivity) _movable.Move(direction.normalized);
    }
}