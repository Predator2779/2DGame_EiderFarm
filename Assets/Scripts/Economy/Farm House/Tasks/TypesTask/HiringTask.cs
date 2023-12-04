using System.Linq;
using Characters;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/EmployeeTasks/HiringTask", fileName = "New HiringTask", order = 0)]
    public class HiringTask : Task
    {
        [Header("Task Cell")]
        // [SerializeField] private CollectTaskCell _cellPrefab;
        [Header("Employees")]
        [SerializeField] private Employee[] _requiredEmployees;
        
        protected override void Initialize()
        {
            throw new System.NotImplementedException();
        }

        protected override void Deinitialize()
        {
            throw new System.NotImplementedException();
        }

        public override void CreateCell(Transform parent)
        {
            // Instantiate(_cellPrefab, parent).SetCell(this);
        }

        protected override bool SomeCondition() => _requiredEmployees.All(e => !e.CanHiring());
    }
}