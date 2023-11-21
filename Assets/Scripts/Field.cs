using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    [SerializeField] private GameObject _prefabGaga;
    [SerializeField] private GameObject[] _spawnPlaces;
    private bool[] _occupiedSpawnPlaces;

    [SerializeField] private Transform[] _center;
    [SerializeField] private float _maxRadius;

    [Header("Задержка перед спавном гаги.")]
    [SerializeField, Range(0, 30)] private int _spawnDelay;

    private List<Gaga> _gagas;


    private void OnEnable() => Initialize();

    private void Initialize()
    {
        _gagas = new List<Gaga>();
        _occupiedSpawnPlaces = new bool[_spawnPlaces.Length];
        GagaSpawn(4);
    }

    public GameObject GetRandomSpawnPlace()
    {
        int randomPlaceNumber = Random.Range(0, _spawnPlaces.Length);
        if (!_occupiedSpawnPlaces[randomPlaceNumber])
        {
            _occupiedSpawnPlaces[randomPlaceNumber] = true;
            return _spawnPlaces[randomPlaceNumber];
        }
        while (_occupiedSpawnPlaces[randomPlaceNumber])
        {
            randomPlaceNumber = Random.Range(0, _spawnPlaces.Length);
            if (!_occupiedSpawnPlaces[randomPlaceNumber])
            {
                _occupiedSpawnPlaces[randomPlaceNumber] = true;
                break;
            }
        }
        return _spawnPlaces[randomPlaceNumber];
    }

    public void GagaSpawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_prefabGaga != null)
            {
                _gagas.Add(Instantiate(_prefabGaga, GetRandomSpawnPlace().transform.position, Quaternion.identity)
                        .GetComponent<Gaga>());
                _gagas[i].Initialize(_center[Random.Range(0, _center.Length)], Random.Range(10, _maxRadius));
            }
        }
    }
}