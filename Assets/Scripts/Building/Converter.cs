using Economy;
using UnityEngine;

public class Converter : MonoBehaviour
{
    [SerializeField] private Item _relevantItem;
    [SerializeField] private Item _convertedItem;

    public Item GetRelevantItem() => _relevantItem;

    public Item Convert(Item item, BuildStorage storage) => item == _convertedItem ? _relevantItem : null;
}