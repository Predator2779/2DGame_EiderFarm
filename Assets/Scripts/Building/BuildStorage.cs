using UnityEngine;

public class BuildStorage : MonoBehaviour
{
    [Header("Количество пуха.")]
    [SerializeField] private int _fluffCount;

    public void AddFluff() => _fluffCount++;
    public void AddFluff(int fluff) => _fluffCount += fluff;

    public int GetFluffCount() => _fluffCount;

    public void ResetFluff() => _fluffCount = 0;
}