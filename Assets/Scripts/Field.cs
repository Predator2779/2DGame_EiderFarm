using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    [SerializeField] private GameObject _prefabGaga;
    [SerializeField] private GameObject[] _spawnPlaces;

    [Header("Задержка перед спавном гаги.")]
    [SerializeField, Range(0, 30)] private int _spawnDelay;

    private BuildStorage _buildStorage;
    private Gaga gaga;

    private void OnEnable() => Initialize();

    private void Initialize()
    {
        _buildStorage = GetComponent<BuildStorage>();
        GagaSpawn();
    }

    public GameObject GetRandomSpawnPlace() => _spawnPlaces[Random.Range(0, _spawnPlaces.Length)];

    public Gaga GagaSpawn()
    {
        if (_prefabGaga != null)
        {
            gaga = Instantiate(_prefabGaga, GetRandomSpawnPlace().transform.position, Quaternion.identity)
                    .GetComponent<Gaga>();
            gaga.Initialize(this.gameObject, GetRandomSpawnPlace());
            gaga.GagaDieEvent += GagaDie;
            gaga.GetComponent<FluffGiver>().FluffGiveEvent += () => _buildStorage.AddFluff();
            return gaga;
        }

        return gaga;
    }

    public Gaga GetGaga() => gaga;

    public IEnumerator SpawnGagasWithDelay()
    {
        yield return new WaitForSecondsRealtime(_spawnDelay);
        GagaSpawn();
    }

    public void GagaDie()
    {
        gaga.GagaDieEvent -= GagaDie;
        StartCoroutine(SpawnGagasWithDelay());
    }

    private void OnDestroy() => Destroy(GetGaga().gameObject);
}