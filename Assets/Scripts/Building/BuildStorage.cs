using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildStorage : MonoBehaviour
{

    [Header("Количество пуха.")]
    [SerializeField] private int _fluffCount;

    public void AddFluff()
    {
        _fluffCount++;
    }

    public int GetFluffFromStorage()
    {
        return _fluffCount;
    }

    public void ResetFluff()
    {
        _fluffCount = 0;
    }
}
