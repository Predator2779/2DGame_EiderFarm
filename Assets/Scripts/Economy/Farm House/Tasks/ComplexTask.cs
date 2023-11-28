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

        public override void CreateCell(Transform parent)
        {
            var task = Instantiate(_cellPrefab, parent);
            task.SetCell(this);
            
            DrawSubtasks(_subTasks, task.transform);
        }
        
        private void DrawSubtasks(Task[] tasks, Transform parent)
        {
            foreach (var task in tasks)
                task.CreateCell(parent);
        }
        
        protected override bool SomeCondition() => 
                _subTasks.All(task => task.GetStage() == TaskStage.Completed ||
                                      task.GetStage() == TaskStage.Passed); 

        public override void CheckProgressing()
        {
            foreach (var task in _subTasks)
                task.CheckProgressing();
            
            base.CheckProgressing();
        }
        
        [ContextMenu("Reset Task")] public override void ResetTask()
        {
            foreach (var task in _subTasks)
                task.ResetTask();
        }
    }
}