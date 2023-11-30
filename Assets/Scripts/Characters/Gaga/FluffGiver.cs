using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BuildStorage))]
[RequireComponent(typeof(ResourceTransmitter))]
public class FluffGiver : MonoBehaviour
{
    [Header("Шанс выпадения пуха (в процентах).")]
    [SerializeField] private int _chance;

    [Header("Время выпадения пуха.")]
    [SerializeField] private float _time;

    [Header("Количество воспроизводимого пуха.")]
    [SerializeField] private int _fluffCount;

    private BuildStorage _storage;
    private ResourceTransmitter _transmitter;
    private bool hasGivenFluff;

    private void Start()
    {
        _storage = GetComponent<BuildStorage>();
        _transmitter = GetComponent<ResourceTransmitter>();
        
        StartCoroutine(CreateFluff());
    }
    
    private void GiveFluff()
    {
        if (hasGivenFluff) return;
        
        hasGivenFluff = true;
        if (Random.Range(0, 100) < _chance)
        {
            _storage.AddFluff(_fluffCount);
            _transmitter.CheckBag();
        }
        hasGivenFluff = false;
    }

    private IEnumerator CreateFluff()
    {
        yield return new WaitForSecondsRealtime(_time);
        GiveFluff();
        StartCoroutine(CreateFluff());
    }
}
