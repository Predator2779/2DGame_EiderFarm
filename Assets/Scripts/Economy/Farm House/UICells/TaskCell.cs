using UnityEngine;

namespace Economy.Farm_House
{
    public class TaskCell : MenuCell
    {
        private Task _task;
        
        public void SetCell(Task task)
        {
            _task = task;
            RefreshButton();
        }

        public void TakeTask() => _task.SetStage(TaskStage.Progressing);

        public void RefuseTask() => _task.SetStage(TaskStage.NotStarted);
        
        private void SetButton(Sprite icon, string description, int requireCount, int currentCount)
        {
            SetButton(icon, description);
            _counter.text = $"{currentCount}/{requireCount}";
        }

        private void RefreshButton()
        {
            CollectTask collectTask = (CollectTask)_task;//
            
            SetButton(
                    collectTask.GetIcon(), 
                    collectTask.GetDescription(), 
                    collectTask.GetRequireCount(), 
                    collectTask.GetCurrentCount());
        }
    }
}