using Characters;
using General;
using UnityEngine;

namespace Economy.Farm_House
{
    public class EmployeeCell : MenuCell
    {
        private Employee _employee;
        private Vector2 _spawnPosition;
        private Inventory _playerInventory;
        
        public void SetCell(
                Employee employee, 
                Vector2 spawnPos,
                Inventory playerInventory)
        {
            _employee = employee;
            _spawnPosition = spawnPos;
            _playerInventory = playerInventory;
            
            RefreshButton();
        }

        public void Hire()
        {
            if (!IsEnoughMoney(_playerInventory, _employee.GetPrice()) ||
                !_employee.CanHiring()) return;
            
            _employee.Hire(_playerInventory, _spawnPosition);
            Destroy(gameObject);
        }

        private void RefreshButton() => SetButton(
                _employee.GetSprite(),
                _employee.GetName(),
                _employee.GetDescription(),
                _employee.GetPrice()
        );

        private bool IsEnoughMoney(Inventory inv, int value) =>
                inv.IsExistsItems(GlobalConstants.Money, value);
    }
}