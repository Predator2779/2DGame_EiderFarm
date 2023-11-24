using System.Linq;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Economy.Farm_House
{
    public class TaskHandler : DisplayMenu
    {
        [SerializeField] private CollectTask[] _tasks;
        [SerializeField] private CollectTaskCell collectTaskCellPrefab;

        private void Awake()
        {
            EventHandler.OnTaskStageChanged.AddListener(RefreshTasksStatus);
            EventHandler.OnGiveReward.AddListener(CheckStage);
        }

        private void CheckStage(Task task, TaskStage stage)
        {
            if (stage == TaskStage.Completed)
            {
                task.GiveReward(_playerInventory);
                task.SetStage(TaskStage.Passed);
            }
            
            RefreshTasksStatus(task, stage);
        }

        private void RefreshTasksStatus(Task task, TaskStage stage) => Draw();
        
        public override void Draw()
        {
            ClearContent();

            if (_isHouseMenu)
            {
                DrawTasks(GetTasks(TaskStage.NotStarted));
                return;
            }

            DrawTasks(GetTasks(TaskStage.Progressing));
        }

        private CollectTask[] GetTasks(TaskStage stage) => _tasks.Where(task => task.GetStage() == stage).ToArray();

        private void DrawTasks(CollectTask[] tasks)
        {
            foreach (var task in tasks)
                SetCell(Instantiate(collectTaskCellPrefab, _content), task);
        }

        private void SetCell(CollectTaskCell cell, CollectTask task) => cell.SetCell(task);
    }
}