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
            ResetTasks(); /// убрать на билдинге
            
            EventHandler.OnTaskStageChanged.AddListener(RefreshTasksStatus);
            EventHandler.OnGiveReward.AddListener(GiveReward);
        }
        
        private void GiveReward(Task task, TaskStage stage)
        {
            if (stage == TaskStage.Completed)
                task.GiveReward(_playerInventory);

            RefreshTasksStatus(task, stage);
        }

        private void RefreshTasksStatus(Task task, TaskStage stage) => RefreshDisplay();

        public override void Draw()
        {
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

        private Task[] GetTasks(TaskStage stage) => _tasks.Where(task => task.GetStage() == stage).ToArray();
        
        private void DrawTasks(Task[] tasks)
        {
            foreach (var task in tasks)
                task.CreateCell(_content);
        }

        private void ResetTasks()
        {
            foreach (var task in _tasks)
                task.ResetTask();
        }
    }
}