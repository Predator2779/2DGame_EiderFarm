using System.Collections;
using General;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Other
{
    [RequireComponent(typeof(Camera))]
    public class CamShake : MonoBehaviour
    {
        [SerializeField] private float _duration, _magnitude, _noize;
        [SerializeField] private bool _enable;
        
        private void Update()
        {
            if (_enable)
            {
                Shake();
                _enable = false;
            }
        }
        
        private void Start() => EventHandler.OnCameraShake.AddListener(Shake);
        private void Shake() => ShakeCamera(_duration, _magnitude, _noize);

        private void ShakeCamera(float duration, float magnitude, float noize) =>
                StartCoroutine(ShakeCameraCor(duration, magnitude, noize));

        private void RotateCamera(float duration, float noize) =>
                StartCoroutine(ShakeRotateCor(duration, noize));

        private IEnumerator ShakeRotateCor(float duration, float noize)
        {
            float elapsed = 0f;

            float angleDeg = 30; //
            Vector2 direction = Vector2.left;

            float halfDuration = duration / 2;
            direction = direction.normalized;
            while (elapsed < duration)
            {
                Vector2 currentDirection = direction;
                float t = elapsed < halfDuration ? elapsed / halfDuration : (duration - elapsed) / halfDuration;
                float currentAngle = Mathf.Lerp(0f, angleDeg, t); //
                currentDirection *= Mathf.Tan(currentAngle * Mathf.Deg2Rad);
                Vector2 resDirection = ((Vector3)currentDirection + Vector3.forward).normalized;

                Quaternion.FromToRotation(Vector3.forward, resDirection);

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator ShakeCameraCor(float duration, float magnitude, float noize)
        {
            float elapsed = 0f;

            Vector3 startPosition = transform.localPosition;

            Vector2 noizeStartPoint0 = Random.insideUnitCircle * noize * Random.Range(-1000, 1000);
            Vector2 noizeStartPoint1 = Random.insideUnitCircle * noize * Random.Range(-1000, 1000);

            while (elapsed < duration)
            {
                Vector2 currentNoizePoint0 = Vector2.Lerp(noizeStartPoint0, Vector2.zero, elapsed / duration);
                Vector2 currentNoizePoint1 = Vector2.Lerp(noizeStartPoint1, Vector2.zero, elapsed / duration);

                Vector2 cameraPostionDelta = new Vector2(Mathf.PerlinNoise(currentNoizePoint0.x, currentNoizePoint0.y),
                        Mathf.PerlinNoise(currentNoizePoint1.x, currentNoizePoint1.y));

                cameraPostionDelta *= magnitude;

                transform.localPosition = startPosition + (Vector3)cameraPostionDelta;

                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}