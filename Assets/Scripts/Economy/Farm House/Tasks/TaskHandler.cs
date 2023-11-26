using System.Linq;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Economy.Farm_House
{
    public class TaskHandler : DisplayMenu
    {
        [SerializeField] private CollectTask[] _tasks;
        [SerializeField] private CreateBuildingTask[] _tasksBuilding;
        [SerializeField] private CollectTaskCell _collectTaskCellPrefab;
        [SerializeField] private CreateBuildingTaskCell _buildingTaskCellPrefab;

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

        private void RefreshTasksStatus(Task task, TaskStage stage) => Draw();

        public override void Draw()
        {
            ClearContent();

            if (_isHouseMenu)
            {
                DrawTasks(GetTasks(TaskStage.NotStarted), GetBuildingTasks(TaskStage.NotStarted));
                return;
            }

            DrawTasks(GetTasks(TaskStage.Completed), GetBuildingTasks(TaskStage.Completed));
            DrawTasks(GetTasks(TaskStage.Progressing), GetBuildingTasks(TaskStage.Progressing));
        }

        private CollectTask[] GetTasks(TaskStage stage) => _tasks.Where(task => task.GetStage() == stage).ToArray();

        private CreateBuildingTask[] GetBuildingTasks(TaskStage stage) => _tasksBuilding.Where(task => task.GetStage() == stage).ToArray();

        private void DrawTasks(CollectTask[] tasks, CreateBuildingTask[] tasksBuilding)
        {
            foreach (var task in tasks)
                SetCell(Instantiate(_collectTaskCellPrefab, _content), task);

            foreach (var taskBuilding in tasksBuilding)
                SetCellTwo(Instantiate(_buildingTaskCellPrefab, _content), taskBuilding);
        }

        private void SetCell(CollectTaskCell cell, CollectTask task) => cell.SetCell(task);
        private void SetCellTwo(CreateBuildingTaskCell cell, CreateBuildingTask task) => cell.SetCell(task);

        private void ResetTasks()
        {
            foreach (var task in _tasks)
                task.ResetTask();
        }
    }
}