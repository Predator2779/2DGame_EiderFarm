using UnityEngine;

namespace Characters
{
    public class Walker : MonoBehaviour, IMovable
    {
        [Header("Character speed.")]
        [SerializeField][Range(0, 10)] private float _speed;
        [SerializeField][Range(0, 15)] private float _runSpeed;

        private Movement _movement;
        
        private void OnValidate() => SetNullableFields();
        private void Start() => SetNullableFields();
        private void SetNullableFields() => _movement ??= GetComponent<Movement>();
        public void Walk(Vector2 direction) => Move(direction, _speed);

        public void Run(Vector2 direction) => Move(direction, _runSpeed);

        private void Move(Vector2 direction, float speed)
        {
            _movement.Move(direction * speed * Time.deltaTime);
        }
    }
}