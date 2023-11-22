using System.Collections;
using TMPro;
using UnityEngine;

namespace General.Game_Settings
{
    public class FpsLimiter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fpsCounter;
        [SerializeField] private int _fpsLimit;
        [SerializeField] private float _delay;

        private float deltaTime;
        
        private void Awake() => Application.targetFrameRate = _fpsLimit;

        private IEnumerator Start()
        {
            while (true)
            {
                if (Time.timeScale == 1)
                {
                    var fps = 1.0f / Time.deltaTime;
                    _fpsCounter.text = "FPS: " + (int)fps;
                }

                yield return new WaitForSeconds(_delay);
            }
        }
    }
}