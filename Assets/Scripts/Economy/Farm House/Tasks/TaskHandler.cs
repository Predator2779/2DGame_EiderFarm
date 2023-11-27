using System.Linq;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Economy.Farm_House
{
    public class TaskHandler : DisplayMenu
    {
        [SerializeField] private CollectTask[] _tasks;

        private void Awake()
        {
            ResetTasks();
            
            EventHandler.OnTaskStageChanged.AddListener(RefreshTasksStatus);
            EventHandler.OnGiveReward.AddListener(GiveReward);
        }

        private void GiveReward(Task task, TaskStage stage)
        {
            if (stage == TaskStage.Passed)
                task.GiveReward(_playerInventory);

            RefreshTasksStatus(task, stage);
        }

        private void RefreshTasksStatus(Task task, TaskStage stage) => RefreshDisplay();

        public override void Draw()
        {
            // ClearContent();

            if (_isHouseMenu)
            {
                SetBtnText("Доступные");
                DrawTasks(GetTasks(TaskStage.NotStarted));
                return;
            }

            SetBtnText("Мои");
            DrawTasks(GetTasks(TaskStage.Completed));
            DrawTasks(GetTasks(TaskStage.Progressing));
        }

        private CollectTask[] GetTasks(TaskStage stage) => _tasks.Where(task => task.GetStage() == stage).ToArray();
        
        private void DrawTasks(CollectTask[] tasks)
        {
            foreach (var task in tasks)
                SetCell(Instantiate(task.GetCell(), _content), task);
        }

        private void SetCell(CollectTaskCell cell, CollectTask task) => cell.SetCell(task);

        private void ResetTasks()
        {
            foreach (var task in _tasks)
                task.ResetTask();
        }
    }
}