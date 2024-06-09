using System.Linq;
using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/CollectTasks/ComplexTask", fileName = "New ComplexTask", order = 0)]
    public class ComplexTask : Task
    {
        [Header("Task Cell")]
        [SerializeField] protected ComplexTaskCell _cellPrefab;
        [Header("Subtasks")]
        [SerializeField] private Task[] _subTasks;

        protected override void Initialize() => EventHandler.OnTaskStageChanged.AddListener(CheckSubtasks);
        protected override void Deinitialize() => EventHandler.OnTaskStageChanged.RemoveListener(CheckSubtasks);

        public override void StartTask()
        {
            base.StartTask();

            foreach (var sub in _subTasks)
                sub.StartTask();
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

        protected override bool SomeCondition() =>
                _subTasks.All(sub => sub.GetStage() == TaskStage.Completed ||
                                     sub.GetStage() == TaskStage.Passed);

        [ContextMenu("Reset Task")] public override void ResetTask()
        {
            base.ResetTask();

            foreach (var task in _subTasks)
                task.ResetTask();
        }
    }
}