using Economy;
using Economy.Farm_House;
using UnityEngine.Events;

namespace General
{
    public static class EventHandler
    {
        public static UnityEvent<string, int> OnInventoryAdd = new();
        public static UnityEvent<Item> OnItemPickUp = new();
        public static UnityEvent<Item> OnItemPut = new();
        public static UnityEvent<Task, TaskStage> OnTaskStageChanged = new();
        public static UnityEvent<Task, TaskStage> OnGiveReward = new();
    }
}