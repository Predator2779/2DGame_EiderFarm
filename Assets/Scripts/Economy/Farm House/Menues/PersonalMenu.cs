using Characters;
using UnityEngine;

namespace Economy.Farm_House
{
    public class PersonalMenu : DisplayMenu
    {
        [SerializeField] private EmployeeCell _cellPrefab;
        [SerializeField] private Vector2 _spawnPosition;
        
        private Employee[] _personal;
        
        public override void Draw()
        {
            foreach (var employee in _personal)
                if (employee.CanHiring())
                    SetCell(Instantiate(_cellPrefab, _content), employee);
        }

        private void SetCell(EmployeeCell cell, Employee employee) =>
                cell.SetCell(employee, _spawnPosition, _playerInventory);
    }
}