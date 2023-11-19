using UnityEngine.Events;

namespace General
{
    public static class EventHandler
    {
        public static UnityEvent<string, int> OnInventoryAdd = new();
    }
}