using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/CollectTask", fileName = "New CollectTask", order = 0)]
    public class CollectTask : Task
    {
        [SerializeField] private Item _requiredItem;
        [SerializeField] protected int _requireCount;
        [SerializeField] protected int _currentCount;

        protected override void Initialize()
        {
            EventHandler.OnItemPickUp.AddListener(PickUpItem);
            EventHandler.OnItemPut.AddListener(PutItem);
        }

        protected override void Deinitialize()
        {
            EventHandler.OnItemPickUp.RemoveListener(PickUpItem);
            EventHandler.OnItemPut.RemoveListener(PutItem);
        }

        public int GetRequireCount() => _requireCount;
        public int GetCurrentCount() => _currentCount;

        private void PickUpItem(Item item, int count)
        {
            if (_requiredItem == item) 
                _currentCount += count;
            
            CheckProgressing();
        }

        private void PutItem(Item item, int count)
        {
            if (_requiredItem == item) 
                _currentCount -= count;
            
            CheckProgressing();
        }

        protected override bool SomeCondition() => _currentCount >= _requireCount;

        [ContextMenu("Reset Task")] public override void ResetTask()
        {
            _currentCount = 0;
            SetStage(TaskStage.NotStarted);
        }
    }
}