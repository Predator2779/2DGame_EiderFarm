using System.Linq;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/CollectTasks/ComplexTask", fileName = "New ComplexTask", order = 0)]
    public class ComplexTask : Task
    {
        [SerializeField] protected ComplexTaskCell _cellPrefab;
        [SerializeField] private Task[] _subTasks;
        
        protected override void Initialize() { }

        protected override void Deinitialize() { }

        public override void SetCell(Transform parent) => 
                Instantiate(_cellPrefab, parent).SetCell(this);
        
        protected override bool SomeCondition() => 
                _subTasks.All(task => task.GetStage() == TaskStage.Completed);

        public override void CheckProgressing()
        {
            foreach (var task in _subTasks)
                task.CheckProgressing();
         
            DrawSubtasks();
            
            base.CheckProgressing();
        }

        private void DrawSubtasks()
        {
            /// Draw...
        }
        
        public override void ResetTask()
        {
            foreach (var task in _subTasks)
                task.ResetTask();
        }
    }
}