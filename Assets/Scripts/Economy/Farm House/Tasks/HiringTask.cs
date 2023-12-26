using System.Linq;
using Characters;
using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/EmployeeTasks/HiringTask", fileName = "New HiringTask", order = 0)]
    public class HiringTask : Task
    {
        [Header("Task Cell")]
        [SerializeField] private GeneralCell _cellPrefab;
        [Header("Employees")]
        [SerializeField] private Employee[] _requiredEmployees;

        protected override void Initialize()
        {
            EventHandler.OnEmployeeHired.AddListener(ProgressingTask);
            ProgressingTask();
        }

        protected override void Deinitialize() =>
                EventHandler.OnEmployeeHired.RemoveListener(ProgressingTask);

        public override void CreateCell(Transform parent) =>
                Instantiate(_cellPrefab, parent).SetCell(this);

        protected override bool SomeCondition() => _requiredEmployees.All(e => !e.CanHiring());

        private void ScanEmployees()
        {
            
        }
    }
}