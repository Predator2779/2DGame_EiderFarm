using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/CollectTask", fileName = "New Task", order = 0)]
    public class CollectTask : Task
    {
        [SerializeField] private Item _requiredItem;
        [SerializeField] private int _requireCount;
        [SerializeField] private int _currentCount;

        public void Initialize()
        {
            EventHandler.OnItemPickUp.AddListener(PickUpItem);
            EventHandler.OnItemPut.AddListener(PutItem);
        }

        public void Reinitialize()
        {
            Deinitialize();
            Initialize();
        }

        public void Deinitialize()
        {
            EventHandler.OnItemPickUp.RemoveListener(PickUpItem);
            EventHandler.OnItemPut.RemoveListener(PutItem);
        }

        public int GetRequireCount() => _requireCount;
        public int GetCurrentCount() => _currentCount;
        
        private void PickUpItem(Item item, int count)
        {
            if (_requiredItem == item) _currentCount += count;
            CheckProgressing();
        }  
        
        private void PutItem(Item item, int count)
        {
            if (_requiredItem == item) _currentCount -= count;
            CheckProgressing();
        }

        private void CheckProgressing()
        {
            if (_stage == TaskStage.Progressing &&
                _currentCount >= _requireCount) 
                SetStage(TaskStage.Completed);
        }

        [ContextMenu("Reset Task")] public void ResetTask()
        {
            _currentCount = 0;
            SetStage(TaskStage.NotStarted);
        }
    }
}