using Economy;
using Economy.Farm_House;
using UnityEngine.Events;

namespace General
{
    public static class EventHandler
    {
        public static UnityEvent<string, int> OnBunchChanged = new(); // for UI

        #region Tasks

        public static UnityEvent<Item, int> OnItemPickUp = new();
        public static UnityEvent<Item, int> OnItemPut = new();

        public static UnityEvent<GlobalTypes.TypeBuildings> OnBuilded = new();
        public static UnityEvent OnFlagSet = new();
        
        public static UnityEvent<Task, TaskStage> OnTaskStageChanged = new();
        public static UnityEvent<Task, TaskStage> OnGiveReward = new();

        #endregion
    }
}