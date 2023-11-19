using Economy.Items;
using UnityEngine;

public class Converter : MonoBehaviour
{
    public ItemType Convert(ItemType from)
    {
        switch (from)
        {
            case ItemType.CleanedFluff:
                return ItemType.Item;
            case ItemType.UncleanedFluff:
                return ItemType.CleanedFluff;
            case ItemType.Item:
                return ItemType.Money;
            default:
                return ItemType.UncleanedFluff;
        }
    }
}