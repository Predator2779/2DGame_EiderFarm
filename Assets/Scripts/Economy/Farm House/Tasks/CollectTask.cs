using UnityEngine;

namespace Economy.Farm_House
{
    public abstract class CollectTask : Task
    {
        [Header("Task Cell")]
        [SerializeField] protected CollectTaskCell _cellPrefab;
        
        [Header("Counters")]
        [SerializeField] protected int _requireCount;
        [SerializeField] protected int _currentCount;
        
        public CollectTaskCell GetCell() => _cellPrefab;
        
        public override void SetCell(Transform parent) => 
                Instantiate(_cellPrefab, parent).SetCell(this);
        
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