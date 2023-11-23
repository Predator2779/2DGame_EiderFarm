using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
    public class PersonAnimate : MonoBehaviour
    {
        private Animator _animator;
        private TurnHandler _turnHandler;

        private void Awake() => Initialize();

        private void Initialize()
        {
            _animator ??= GetComponent<Animator>();
            _turnHandler ??= new TurnHandler(GetComponent<SpriteRenderer>());
        }

        public void Walk(Vector2 direction, bool isWalk)
        {
            _animator.SetBool("isWalk", isWalk);
            _animator.SetFloat("dirY", direction.y);
            
            CheckSide(direction);
        }

        private void CheckSide(Vector2 direction)
        {
            switch (direction.x)
            {
                case > 0:
                    _turnHandler.ChangeSide(TurnHandler.PlayerSides.Right);
                    break;
                case < 0:
                    _turnHandler.ChangeSide(TurnHandler.PlayerSides.Left);
                    break;
            }
        }
    }
}