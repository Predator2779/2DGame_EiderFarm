using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/CollectTasks/ProcessingTask", fileName = "New ProcessingTask", order = 0)]
    public class ProcessingTask : CollectTask
    {
        [SerializeField] protected GlobalTypes.TypeBuildings _requireType;
        
        protected override void Initialize()
        {
            EventHandler.OnItemTransmitted.AddListener(CheckTransmitte);
        }

        protected override void Deinitialize()
        {
            EventHandler.OnItemTransmitted.RemoveListener(CheckTransmitte);
        }

        private void CheckTransmitte(GlobalTypes.TypeBuildings type, int count)
        {
            if (type == _requireType) AddCount(count);
            
            ProgressingTask();
        }
        
        [ContextMenu("Reset Task")] public override void ResetTask()
        {
            base.ResetTask();
            
            _currentCount = 0;
        }
    }
}