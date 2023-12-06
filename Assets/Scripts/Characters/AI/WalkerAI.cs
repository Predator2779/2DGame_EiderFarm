using Player;
using UnityEngine;

namespace Characters.AI
{
    public abstract class WalkerAI : MonoBehaviour
    {
        [SerializeField] protected PersonAnimate _personAnimate;
        [SerializeField] protected Walker _walker;
        
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