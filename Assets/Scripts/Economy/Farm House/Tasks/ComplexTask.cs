using System.Linq;
using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/CollectTasks/ComplexTask", fileName = "New ComplexTask", order = 0)]
    public class ComplexTask : Task
    {
        [SerializeField] protected ComplexTaskCell _cellPrefab;
        [SerializeField] private Task[] _subTasks;

        protected override void Initialize()
        {
            EventHandler.OnTaskStageChanged.AddListener(CheckSubtasks);
        }

        protected override void Deinitialize()
        {
            EventHandler.OnTaskStageChanged.RemoveListener(CheckSubtasks);
        }

        private void CheckSubtasks(Task task, TaskStage stage)
        {
            if (_subTasks.Any(t => t == task) &&
                stage == TaskStage.Completed)
                ProgressingTask();
        }

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

        protected override bool SomeCondition()
        {
            return _subTasks.All(sub => sub.GetStage() == TaskStage.Completed || sub.GetStage() == TaskStage.Passed);
        }
                // _subTasks.All(task => task.GetStage() == TaskStage.Completed);

        public override void CheckProgressing()
        {
            foreach (var task in _subTasks)
                task.CheckProgressing();

            base.CheckProgressing();
        }

        [ContextMenu("Reset Task")] public override void ResetTask()
        {
            base.ResetTask();

            foreach (var task in _subTasks)
                task.ResetTask();
        }
    }
}