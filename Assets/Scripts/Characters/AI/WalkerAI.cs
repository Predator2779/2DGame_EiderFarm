using FMOD.Studio;
using FMODUnity;
using Player;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Characters.AI
{
    [RequireComponent(typeof(Walker))]
    public abstract class WalkerAI : MonoBehaviour
    {
        [SerializeField] protected PersonAnimate _personAnimate;
        [SerializeField] private string _walkSound;

        private EventInstance _eventInstance;
        private Walker _walker;
        private bool _isPlayed;

        private void Awake()
        {
            _walker = GetComponent<Walker>();
            _eventInstance = RuntimeManager.CreateInstance(_walkSound);
        }

        protected void Walk(Vector2 direction)
        {
            _walker.Walk(direction);
            Animate(direction);
        }

        protected virtual void Run(Vector2 direction)
        {
            _walker.Run(direction);
            Animate(direction);
        }

        private void Animate(Vector2 direction) => _personAnimate.Walk(direction, true);

        protected void PlaySound()
        {
            if (_isPlayed) return;
            
            _eventInstance.start();
            _isPlayed = true;
        }

        protected void StopSound()
        {
            if (!_isPlayed) return;
            
            _eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _isPlayed = false;
        }
    }
}