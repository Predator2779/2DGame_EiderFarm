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

        public override void CreateCell(Transform parent) =>
                Instantiate(_cellPrefab, parent).SetCell(this);

        protected void AddCount(int value)
        {
            _currentCount += value;
            
            if (_currentCount > _requireCount)
                _currentCount = _requireCount;
        }

        protected void RemoveCount(int value)
        {
            _currentCount -= value;

            if (_currentCount < 0)
                _currentCount = 0;
        }

        public int GetRequireCount() => _requireCount;
        public int GetCurrentCount() => _currentCount;

        protected override bool SomeCondition() => _currentCount >= _requireCount;

        [ContextMenu("Reset Task")] public override void ResetTask()
        {
            base.ResetTask();
            
            _currentCount = 0;
        }
    }
}