using UnityEngine;
using EventHandler = General.EventHandler;

namespace Economy.Farm_House
{
    public abstract class Task : ScriptableObject
    {
        [Header("Task")]
        [SerializeField] protected TaskStage _stage;
        [SerializeField] protected TaskStage _resetStage;
        [SerializeField] protected Sprite _icon;
        [SerializeField] protected string _name;
        [SerializeField] protected string _description;
        
        [Header("Reward")]
        [SerializeField] protected ItemBunch _reward;

        [Header("Set available tasks")]
        [SerializeField] protected Task[] _nextTasks;
        
        protected abstract void Initialize();

        private void Reinitialize()
        {
            Deinitialize();
            Initialize();
        }
        
        protected abstract void Deinitialize();

        public abstract void CreateCell(Transform parent);
        
        public Sprite GetIcon() => _icon;
        public string GetName() => _name;
        public string GetDescription() => _description;

        public TaskStage GetStage() => _stage;

        protected void SetStage(TaskStage stage)
        {
            _stage = stage;
            EventHandler.OnTaskStageChanged?.Invoke(this, _stage);
        }

        public void GiveReward(Inventory inventory) =>
                inventory.AddItems(_reward.GetItem(), _reward.GetCount());

        protected abstract bool SomeCondition();

        private void StartTask()
        {
            Reinitialize();
            SetStage(TaskStage.Progressing);
        }

        protected void ProgressingTask()
        {
            if (SomeCondition()) 
                SetStage(TaskStage.Completed);
        }

        private void PassTask()
        {
            EventHandler.OnGiveReward?.Invoke(this, _stage);
            SetStage(TaskStage.Passed);
            SetAvailableTasks();
            Deinitialize();
        }

        public virtual void CheckProgressing()
        {
            switch (GetStage())
            {
                case TaskStage.NotStarted:
                    StartTask();
                    break;
                case TaskStage.Progressing:
                    ProgressingTask();
                    break;
                case TaskStage.Completed:
                    PassTask();
                    break;
            }
        }

        private void SetAvailableTasks()
        {
            foreach (var task in _nextTasks)
            {
                if (task.GetStage() == TaskStage.NotAvailable)
                    task.SetStage(TaskStage.NotStarted);
            }
        }

        public virtual void ResetTask()
        {
            SetStage(_resetStage);
        }
    }

    public enum TaskStage
    {
        NotAvailable,
        NotStarted,
        Progressing,
        Completed,
        Passed
    }
}