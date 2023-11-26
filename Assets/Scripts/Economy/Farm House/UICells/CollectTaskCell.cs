using UnityEngine;

namespace Economy.Farm_House
{
    public class CollectTaskCell : MenuCell
    {
        private CollectTask _task;
        
        public void SetCell(CollectTask task)
        {
            _task = task;
            RefreshButton();
        }

        public void ClickTaskBtn() => _task.CheckProgressing();
        
        private void SetButton(Sprite icon, string description, int requireCount, int currentCount)
        {
            SetButton(icon, description);
            _counter.text = $"{currentCount}/{requireCount}";
        }

        private void RefreshButton()
        {
            if (_task == null) return;
            
            _task.Reinitialize();
            
            SetButton(
                    _task.GetIcon(), 
                    _task.GetDescription(), 
                    _task.GetRequireCount(), 
                    _task.GetCurrentCount());
        }
    }
}