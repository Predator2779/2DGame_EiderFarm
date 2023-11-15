using Building;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private BuildMenu _buildMenu;

    [SerializeField] private GameObject _prefabGaga;
    [SerializeField] private GameObject[] _spawnPlaces;

    [Header("Задержка перед спавном гаги.")]
    [SerializeField,Range(0,30)] private int _spawnDelay;
    public GameObject GetRandomSpawnPlace() => _spawnPlaces[UnityEngine.Random.Range(0, _spawnPlaces.Length)];

    private Gaga gaga;
    public Gaga GagaSpawn()
    {
        if (_prefabGaga != null)
        {
            gaga = Instantiate(_prefabGaga, GetRandomSpawnPlace().transform.position, Quaternion.identity).GetComponent<Gaga>();
            gaga.Initialize(this.gameObject, GetRandomSpawnPlace());
            gaga.GagaDieEvent += GagaDie;
            gaga.GetComponent<FluffGiver>().FluffGiveEvent += () => _buildMenu.AddFluffToStorage();
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
}
