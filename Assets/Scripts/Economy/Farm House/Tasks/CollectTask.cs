using UnityEngine;

namespace Economy.Farm_House
{
    public abstract class CollectTask : Task
    {
        [SerializeField] protected Item _requiredItem;
        [SerializeField] protected int _requireCount;
        [SerializeField] protected int _currentCount;

        public int GetRequireCount() => _requireCount;
        public int GetCurrentCount() => _currentCount;

        protected override bool SomeCondition() => _currentCount >= _requireCount;

        [ContextMenu("Reset Task")] public override void ResetTask()
        {
            _currentCount = 0;
            SetStage(TaskStage.NotStarted);
        }
    }
}