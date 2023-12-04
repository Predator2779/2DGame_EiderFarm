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

        private void RefreshButton()
        {
            if (_task == null) return;

            SetButton(_task.GetDescription());
        }
    }
}