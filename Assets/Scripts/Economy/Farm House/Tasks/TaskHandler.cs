using System.Linq;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Economy.Farm_House
{
    public class TaskHandler : DisplayMenu
    {
        [SerializeField] private Task[] _tasks;

        public void Initialize()
        {
            EventHandler.OnTaskStageChanged.AddListener(RefreshTasksStatus);
            EventHandler.OnGiveReward.AddListener(GiveReward);

            foreach (var task in _tasks)
                if (task.GetStage() == TaskStage.Progressing)
                {
                    task.StartTask();
                    return;
                }
        }

        private void GiveReward(Task task, TaskStage stage)
        {
            if (stage == TaskStage.Completed)
                task.GiveReward(_playerInventory);

            RefreshDisplay();
        }

        private void RefreshTasksStatus(Task task, TaskStage stage) => RefreshDisplay();

        protected override void Draw()
        {
            DrawTasks(GetTasks(TaskStage.Progressing));
            DrawTasks(GetTasks(TaskStage.Completed));
        }

        private Task[] GetTasks(TaskStage stage) => _tasks.Where(task => task.GetStage() == stage).ToArray();

        private void DrawTasks(Task[] tasks)
        {
            foreach (var task in tasks)
                task.CreateCell(_content);
        }

        public void ResetAllTasks()
        {
            ResetTasks();
            _tasks[0].StartTask();
        }

        public void ResetTasks()
        {
            foreach (var task in _tasks)
                task.ResetTask();
        }
    }
}