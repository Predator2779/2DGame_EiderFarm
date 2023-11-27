using UnityEngine;
using EventHandler = General.EventHandler;

namespace Economy.Farm_House
{
    public abstract class Task : ScriptableObject
    {
        [Header("Task")]
        [SerializeField] protected TaskStage _stage;
        [SerializeField] protected Sprite _icon;
        [SerializeField] protected string _name;
        [SerializeField] protected string _description;
        
        [Header("Reward")]
        [SerializeField] protected ItemBunch _reward;

        protected abstract void Initialize();

        protected abstract void Deinitialize();

        public abstract void SetCell(Transform parent);
        
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
            Initialize();
            SetStage(TaskStage.Progressing);
        }

        private void ProgressingTask()
        {
            if (SomeCondition()) 
                SetStage(TaskStage.Completed);
        }

        private void PassTask()
        {
            SetStage(TaskStage.Passed);
            EventHandler.OnGiveReward?.Invoke(this, _stage);
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

        public virtual void ResetTask()
        {
            
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