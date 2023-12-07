using Player;
using UnityEngine;

namespace Characters.AI
{
    [RequireComponent(typeof(Walker))]
    public abstract class WalkerAI : MonoBehaviour
    {
        [SerializeField] protected PersonAnimate _personAnimate;
        private Walker _walker;
        private void Start() => _walker = GetComponent<Walker>();

        protected void Walk(Vector2 direction)
        {
            _walker.Walk(direction);
            _personAnimate.Walk(direction, true);
        }

        protected virtual void Run(Vector2 direction)
        {
            _walker.Run(direction);
            _personAnimate.Walk(direction, true);
        }
    }
}