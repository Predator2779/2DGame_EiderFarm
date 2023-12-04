using Economy;
using Economy.Farm_House;
using TriggerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace General
{
    public static class EventHandler
    {
        public static UnityEvent<string, int> OnBunchChanged = new(); // for UI

        #region FLAGS

        public static UnityEvent<int, Sprite[]> OnFlagChanged = new();
        public static UnityEvent<bool> FlagPanelEvent = new();
        public static UnityEvent<Sprite> OnFlagSpriteChanged = new();
        public static UnityEvent OnFlagSet = new();

        #endregion
        
        #region BUILDINGS

        public static UnityEvent<GlobalTypes.TypeBuildings> OnBuilded = new();
        public static UnityEvent<GlobalTypes.TypeBuildings, int> OnUpgraded = new();
        public static UnityEvent<GlobalTypes.TypeBuildings> OnDemolition = new();
        
        public static UnityEvent<BuildTrigger, GlobalTypes.TypeBuildings> OnAddedBuildPull = new();
        public static UnityEvent<BuildTrigger, GlobalTypes.TypeBuildings> OnRemovedBuildPull = new();

        #endregion
        
        #region TASKS

        public static UnityEvent<Item, int> OnItemPickUp = new();
        public static UnityEvent<Item, int> OnItemPut = new();
        public static UnityEvent<GlobalTypes.TypeBuildings, Item, int> OnItemTransmitted = new();
        
        public static UnityEvent<Task, TaskStage> OnTaskStageChanged = new();
        public static UnityEvent<Task, TaskStage> OnGiveReward = new();

        #endregion
    }
}