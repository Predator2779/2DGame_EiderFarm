using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluffGiver : MonoBehaviour
{
    public event Action FluffGiveEvent;

    [SerializeField] private bool hasGivenFluff;

    [Header("Шанс выпадения пуха (в процентах).")]
    [SerializeField] private int _chance;

    private void Start() => hasGivenFluff = false;

    // пока без пугалок и отпугивателей
    public void GiveFluff()
    {
        if (!hasGivenFluff)
        {
            hasGivenFluff = true;
            if (UnityEngine.Random.Range(0, 100) < _chance)
            {
                FluffGiveEvent.Invoke();
            }
        }
    }

}
