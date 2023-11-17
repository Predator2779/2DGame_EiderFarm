using Economy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : MonoBehaviour
{
    public BagContent Convert(BagContent from)
    {
        switch (from)
        {
            case BagContent.Money:
                return BagContent.UncleanedFluff;
            case BagContent.CleanedFluff:
                return BagContent.Clothes;
            case BagContent.UncleanedFluff:
                return BagContent.CleanedFluff;
            case BagContent.Clothes:
                return BagContent.Money;
        }
        return BagContent.UncleanedFluff;
    }
}
