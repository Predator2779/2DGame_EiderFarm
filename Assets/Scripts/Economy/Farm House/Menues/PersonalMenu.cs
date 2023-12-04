using Characters;
using UnityEngine;

namespace Economy.Farm_House
{
    public class PersonalMenu : DisplayMenu
    {
        [SerializeField] private EmployeeCell _cellPrefab;
        [SerializeField] private Transform _spawnTransform;
        [SerializeField] private Employee[] _personal;
        
        protected override void Draw()
        {
            foreach (var employee in _personal)
                if (employee.CanHiring())
                    SetCell(Instantiate(_cellPrefab, _content), employee);
        }

        private void SetCell(EmployeeCell cell, Employee employee) =>
                cell.SetCell(employee, _spawnTransform.position, _playerInventory);
    }
}