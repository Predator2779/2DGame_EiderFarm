using FMOD.Studio;
using FMODUnity;
using Player;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

[RequireComponent(typeof(IMovable))]
public class InputHandler : MonoBehaviour
{
    [SerializeField] private MyJoystick _joystick;

    [Header("Joystick sensetivity.")]
    [SerializeField, Range(0, 1)] private float _sensitivity;
    [SerializeField] private PersonAnimate _personAnimate;
    [SerializeField] private string _walkSound;
    
    private EventInstance _eventInstance;
    private IMovable _movable;
    private bool _isPlayed;

    private void Start()
    {
        _movable = GetComponent<IMovable>();
        _eventInstance = RuntimeManager.CreateInstance(_walkSound);
    }

    private void Update()
    {
        Vector3 direction = new Vector2(_joystick.Horizontal, _joystick.Vertical);

        if (direction.magnitude > _sensitivity) _movable.Walk(direction.normalized);
        
        _personAnimate.Walk(direction, 
                !(direction.y == 0 && direction.x == 0));

        switch (_isPlayed)
        {
            case false when direction.y != 0 || direction.x != 0:
                _eventInstance.start();
                _isPlayed = true;
                return;
            case true when direction.y == 0 && direction.x == 0:
                _eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
                _isPlayed = false;
                break;
        }
    }
}