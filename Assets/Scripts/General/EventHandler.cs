using Building.Constructions;
using Economy.Items;
using UnityEngine.Events;

namespace General
{
    public static class EventHandler
    {
        public static UnityEvent<Construction> OnBuildBtnPressed = new();
        public static UnityEvent<Construction> OnDemolishBtnPressed = new();
        public static UnityEvent<Construction> OnUpgradeBtnPressed = new();
        
        public static UnityEvent<ItemType, int> OnInventoryAdd = new();
    }
}