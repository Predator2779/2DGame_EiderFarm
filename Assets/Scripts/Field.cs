using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private GameObject[] _spawnPlaces;
    public GameObject GetRandomSpawnPlace() => _spawnPlaces[Random.Range(0, _spawnPlaces.Length)];
}
