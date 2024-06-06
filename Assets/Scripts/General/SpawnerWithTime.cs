using System.Collections;
using UnityEngine;

namespace General
{
    public class SpawnerWithTime : Spawner
    {
        [SerializeField] private float _spawnInterval;
        [SerializeField] private float _startDelay;

        private void Start() => StartCoroutine(StartTimer(Random.Range(0, _startDelay)));

        private IEnumerator StartTimer(float time)
        {
            yield return new WaitForSeconds(time);
            StartCoroutine(Timer());
        }
        
        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(_spawnInterval);
            Spawn();
            StartCoroutine(Timer());
        }
    }
}