using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/Task", fileName = "New Task", order = 0)]
    public class Task : ScriptableObject
    {
        [SerializeField] private TaskStage _stage;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private int _reward;
        
        public Sprite GetIcon() => _icon;
        public string GetName() => _name;
        public string GetDescription() => _description;
        public int GetReward() => _reward;

        public TaskStage GetStage() => _stage;
        
        public void SetStage(TaskStage stage)
        {
            _stage = stage;
            EventHandler.OnTaskStageChanged?.Invoke();
        }
    }

    public enum TaskStage
    {
        NotStarted,
        Progressing,
        Completed,
        Passed
    }
}