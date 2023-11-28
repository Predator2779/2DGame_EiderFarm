using System;
using System.Collections;
using UnityEngine;

public class FluffGiver : MonoBehaviour
{
    private BuildStorage _storage;
    public event Action FluffGiveEvent;
    private bool hasGivenFluff;

    [Header("Шанс выпадения пуха (в процентах).")]
    [SerializeField] private int _chance;

    [Header("Время выпадения пуха.")]
    [SerializeField] private float _time;

    [Header("Количество воспроизводимого пуха.")]
    [SerializeField] private float _fluffCount;

    private void Start()
    {
        _storage = GetComponent<BuildStorage>();
        StartCoroutine(CreateFluff());
    }

    // пока без пугалок и отпугивателей
    private void GiveFluff()
    {
        if (!hasGivenFluff)
        {
            hasGivenFluff = true;
            if (UnityEngine.Random.Range(0, 100) < _chance)
            {
                _storage.AddFluff();

            }
            hasGivenFluff = false;
        }
    }

    private IEnumerator CreateFluff()
    {
        yield return new WaitForSecondsRealtime(_time);
        GiveFluff();
        StartCoroutine(CreateFluff());
    }

    public void UpgradeFluffGiver()
    {
        _fluffCount++;
    }

}
