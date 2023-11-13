using Building.Constructions;
using UnityEngine.Events;

namespace General
{
    public static class EventHandler
    {
        public static UnityEvent<Construction> OnBuildBtnPressed = new();
        public static UnityEvent<Construction> OnDemolishBtnPressed = new();
        public static UnityEvent<Construction> OnUpgradeBtnPressed = new();
    }
}