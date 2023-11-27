using System.Linq;
using UnityEngine;

namespace Economy.Farm_House
{
    public class ComplexTasks : Task
    {
        // [SerializeField] protected 
        [SerializeField] private Task[] _subTasks;
        
        protected override void Initialize() { }

        protected override void Deinitialize() { }

        protected override bool SomeCondition() => 
                _subTasks.All(task => task.GetStage() == TaskStage.Completed);

        public override void CheckProgressing()
        {
            foreach (var task in _subTasks)
                task.CheckProgressing();
        }

        public override void ResetTask()
        {
            foreach (var task in _subTasks)
                task.ResetTask();
        }
    }
}