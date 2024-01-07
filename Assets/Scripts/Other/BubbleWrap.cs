using General;
using UnityEngine;

namespace Other
{
    public class BubbleWrap : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private string _sound;
        [SerializeField] private bool _camShake;
        
        public void StartBubble()
        {
            if (_particles != null) _particles.Play();
            if (_sound != "") FMODUnity.RuntimeManager.PlayOneShot(_sound);
            if (_camShake) EventHandler.OnCameraShake?.Invoke();
        }
    }
}