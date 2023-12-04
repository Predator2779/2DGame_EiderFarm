using UnityEngine;

namespace Economy.Farm_House
{
    public class GeneralCell : MenuCell
    {
        private Task _task;
        
        public void SetCell(Task task)
        {
            _task = task;
            RefreshButton();
        }
        
        public void ClickTaskBtn() => _task.CheckProgressing();
        
        private void SetButton(Sprite icon, string name, string description, int rewardCount)
        {
            SetButton(icon, name, description);
            _counter.text = $"Награда: {rewardCount}kr";
        }

        private void RefreshButton()
        {
            if (_task == null) return;
            
            SetButton(
                    _task.GetIcon(), 
                    _task.GetName(), 
                    _task.GetDescription(), 
                    _task.RewardCount());
        }
    }
}