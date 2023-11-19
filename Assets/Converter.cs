using Economy;
using UnityEngine;

public class Converter : MonoBehaviour
{
    [SerializeField] private Item _relevantItem;
    [SerializeField] private Item _convertedItem;

    public Item Convert(Item item, BuildStorage storage)
    {
        if (item == _relevantItem) return _convertedItem;

        return null;
    }
}