using Economy;
using Economy.Farm_House;
using UnityEngine;
using UnityEngine.Events;

namespace General
{
    public static class EventHandler
    {
        public static UnityEvent<string, int> OnBunchChanged = new(); // for UI
        public static UnityEvent<int, Sprite[]> OnFlagChanged = new();

        #region Tasks

        public static UnityEvent<Item, int> OnItemPickUp = new();
        public static UnityEvent<Item, int> OnItemPut = new();
        public static UnityEvent<GlobalTypes.TypeBuildings, int> OnItemTransmitted = new();

        public static UnityEvent<GlobalTypes.TypeBuildings> OnBuilded = new();
        public static UnityEvent<GlobalTypes.TypeBuildings, int> OnUpgraded = new();
        public static UnityEvent OnFlagSet = new();
        
        public static UnityEvent<Task, TaskStage> OnTaskStageChanged = new();
        public static UnityEvent<Task, TaskStage> OnGiveReward = new();

        #endregion
    }
}