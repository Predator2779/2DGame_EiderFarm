using Player;
using UnityEngine;

namespace Characters.AI
{
    public abstract class AnimalAI : MonoBehaviour
    {
        [SerializeField] protected PersonAnimate _personAnimate;
        [SerializeField] protected Animal _animal;

        protected abstract void StateExecute();
        protected abstract void Idle();
        
        protected void Walk(Vector2 direction)
        {
            _animal.Walk(direction);
            _personAnimate.Walk(direction, true);
        }
        
        protected virtual void Run(Vector2 direction)
        {
            _animal.Run(direction);
            _personAnimate.Walk(direction, true);
        }

        protected abstract void CheckConditions();
    }
}