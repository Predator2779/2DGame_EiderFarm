using Building.Constructions;
using Economy;
using UnityEngine.Events;

namespace General
{
    public static class EventHandler
    {
        public static UnityEvent<Construction> OnBuildBtnPressed = new();
        public static UnityEvent<Construction> OnDemolishBtnPressed = new();
        public static UnityEvent<Construction> OnUpgradeBtnPressed = new();
        
        public static UnityEvent<BagContent, int> OnBagAdd = new();
    }
}