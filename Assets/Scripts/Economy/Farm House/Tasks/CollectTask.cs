using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/CollectTask", fileName = "New Task", order = 0)]
    public class CollectTask : Task
    {
        [SerializeField] private int _requireCount;
        [SerializeField] private int _currentCount;
        
        public int GetRequireCount() => _requireCount;
        public int GetCurrentCount() => _currentCount;
    }
}