using Player;
using UnityEngine;

namespace Characters.Enemy
{
    public abstract class PersonAI : MonoBehaviour
    {
        [SerializeField] protected PersonAnimate _personAnimate;

        protected Person _person;
        protected abstract void StateExecute();
        protected abstract void Idle();
        
        protected void Walk(Vector2 direction)
        {
            _person.Walk(direction);
            _personAnimate.Walk(direction, true);
        }
        
        protected virtual void Run(Vector2 direction)
        {
            _person.Run(direction);
            _personAnimate.Walk(direction, true);
        }

        protected abstract void CheckConditions();
    }
}