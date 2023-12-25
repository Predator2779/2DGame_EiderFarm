using UnityEngine;
using EventHandler = General.EventHandler;

namespace Other
{
    public class BubbleWrap : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particles;

        private void Start() => EventHandler.OnBubbleWrap.AddListener(StartBubble);

        private void StartBubble(string sound)
        {
            _particles.Play();
            FMODUnity.RuntimeManager.PlayOneShot(sound);
        }
    }
}