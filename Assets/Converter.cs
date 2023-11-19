using Economy.Items;
using UnityEngine;

public class Converter : MonoBehaviour
{
    public ItemType ConvertFromTo(ItemType from)
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
    public ItemType ConvertToFrom(ItemType to)
    {
        switch (to)
        {
            case ItemType.CleanedFluff:
                return ItemType.UncleanedFluff;
            case ItemType.UncleanedFluff:
                return ItemType.Money;
            case ItemType.Money:
                return ItemType.Item;
            default:
                return ItemType.Money;
        }
    }

}