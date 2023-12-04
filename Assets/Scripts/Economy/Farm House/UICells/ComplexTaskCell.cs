namespace Economy.Farm_House
{
    public class ComplexTaskCell : GeneralCell
    {
        private ComplexTask _task;
        
        public void SetCell(ComplexTask task)
        {
            _task = task;
            RefreshButton();
        }

        public void ClickTaskBtn() => _task.CheckProgressing();
        
        private void RefreshButton()
        {
            if (_task == null) return;

            SetButton(_task.GetIcon(), _task.GetDescription());
        }
    }
}