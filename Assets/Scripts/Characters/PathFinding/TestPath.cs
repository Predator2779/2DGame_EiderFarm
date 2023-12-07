using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.PathFinding
{
    public class TestPath : MonoBehaviour
    {
        [SerializeField] private float _delay;
        [SerializeField] private PathFinder _pathFinder;
        [SerializeField] private Transform _pos;

        private List<Vector2> _path;
        private int index = 0;
        
        private void Start()
        {
            _path = _pathFinder.GetPath(_pos.position);
            
            StartCoroutine(Move());
        }
        
        private IEnumerator Move()
        {
            transform.position = _path[index];
            yield return new WaitForSeconds(_delay);
            index++;
            StartCoroutine(Move());
        }
    }
}