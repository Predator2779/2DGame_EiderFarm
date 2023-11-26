using UnityEngine;
using EventHandler = General.EventHandler;

namespace Economy.Farm_House
{
    public abstract class Task : ScriptableObject
    {
        [SerializeField] protected TaskStage _stage;
        [SerializeField] protected Sprite _icon;
        [SerializeField] protected string _name;
        [SerializeField] protected string _description;
        
        [Header("Reward")]
        [SerializeField] protected ItemBunch _reward;
        
        public Sprite GetIcon() => _icon;
        public string GetName() => _name;
        public string GetDescription() => _description;

        public TaskStage GetStage() => _stage;
        
        public void SetStage(TaskStage stage)
        {
            _stage = stage;
            EventHandler.OnTaskStageChanged?.Invoke(this, _stage);
        }

        public void GiveReward(Inventory inventory) => 
                inventory.AddItems(_reward.GetItem(), _reward.GetCount());
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