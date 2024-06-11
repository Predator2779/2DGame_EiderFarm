using System.Linq;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Economy.Farm_House
{
    public class TaskHandler : DisplayMenu
    {
        [SerializeField] private Task[] _tasks;
        private bool _isRefreshing = false;

        public void Initialize()
        {
            EventHandler.OnTaskStageChanged.AddListener(RefreshTasksStatus);
            EventHandler.OnGiveReward.AddListener(GiveReward);

            foreach (var task in _tasks)
            {
                if (task.GetStage() == TaskStage.Progressing)
                {
                    task.StartTask();
                    return;
                }
            }
        }

        private void GiveReward(Task task, TaskStage stage)
        {
            if (stage == TaskStage.Completed && _playerInventory != null)
                task.GiveReward(_playerInventory);
        }

        private void RefreshTasksStatus(Task task, TaskStage stage)
        {
            if (_isRefreshing) return;

            _isRefreshing = true;
            RefreshDisplay();
            _isRefreshing = false;
        }

        protected override void Draw()
        {
            DrawTasks(GetTasks(TaskStage.Progressing));
            DrawTasks(GetTasks(TaskStage.Completed));
        }

        public override void RefreshDisplay()
        {
            if (_isRefreshing) return;

            _isRefreshing = true;
            CheckTasks(GetTasks(TaskStage.Progressing));
            CheckTasks(GetTasks(TaskStage.Completed));
            base.RefreshDisplay();
            _isRefreshing = false;
        }

        private Task[] GetTasks(TaskStage stage) => _tasks.Where(task => task.GetStage() == stage).ToArray();

        private void DrawTasks(Task[] tasks)
        {
            foreach (var task in tasks)
                task.CreateCell(_content);
        }

        private void CheckTasks(Task[] tasks)
        {
            foreach (var task in tasks)
            {
                if (task.GetStage() == TaskStage.Progressing)
                {
                    task.ProgressingTask();
                }
                else if (task.GetStage() == TaskStage.Completed)
                {
                    task.PassTask();
                }
            }
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