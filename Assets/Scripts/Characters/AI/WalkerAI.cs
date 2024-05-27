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
        private Rigidbody2D _rbody;
        
        private void Awake()
        {
            _rbody = GetComponent<Rigidbody2D>();
            _walker = GetComponent<Walker>();
            _eventInstance = RuntimeManager.CreateInstance(_walkSound);
            RuntimeManager.AttachInstanceToGameObject(_eventInstance, transform, _rbody);
        }

        protected void Walk(Vector2 direction)
        {
            _walker.Walk(direction);
            PlaySound();
            _eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, _rbody));
            Animate(direction);
        }
        
        protected virtual void WalkAnimation(Vector2 direction)
        {
            PlaySound();
            _eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, _rbody));
            Animate(direction);
        }

        protected virtual void Run(Vector2 direction)
        {
            _walker.Run(direction);
            PlaySound();
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

            _eventInstance.stop(STOP_MODE.IMMEDIATE);
            _isPlayed = false;
        }
    }
}