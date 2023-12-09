using Characters;
using UnityEngine;

namespace Audio
{
    public class BallAudio : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.GetComponent<Walker>())
                FMODUnity.RuntimeManager.PlayOneShot("event:/”дар по м€чу 2.0");
            else if (other.transform.CompareTag("Obstacle"))
                FMODUnity.RuntimeManager.PlayOneShot("event:/”дар и отскок м€ча (от дерева, забора) 2.0");
            else
                FMODUnity.RuntimeManager.PlayOneShot("event:/”дар и отскок м€ча (не от дерева) 2.0");
        }
    }
}