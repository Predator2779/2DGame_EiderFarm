using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace General
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _spawnObjs;
        [SerializeField] private int _countObjs;
        [SerializeField] private bool _isRandomPos;
        [SerializeField] private float _radius;

        public void Spawn()
        {
            int length = _spawnObjs.Length;

            for (int i = 0; i < length; i++)
                for (int j = 0; j < _countObjs; j++)
                    Instantiate(_spawnObjs[i], GetPosition(), Quaternion.identity);
        }

        private Vector2 GetPosition()
        {
            Vector2 currentPos = transform.position;
            Vector2 position = currentPos;

            if (!_isRandomPos) return position;
            
            position = currentPos + Random.insideUnitCircle * _radius;

            return position;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

#endif
    }
}