using System.Linq;
using Building;
using Characters;
using General;
using TriggerScripts;
using UnityEngine;

namespace Economy.Farm_House
{
    public class PersonalMenu : DisplayMenu
    {
        [SerializeField] private EmployeeCell _cellPrefab;
        [SerializeField] private Employee[] _personal;
        [SerializeField] private BuildingsPull _pull;
        
        private void Start() => _pull ??= FindObjectOfType<BuildingsPull>();
        
        protected override void Draw()
        {
            if (!CanHiring()) return;
            
            foreach (var employee in _personal)
                if (employee.CanHiring())
                    SetCell(Instantiate(_cellPrefab, _content), employee);
        }

        private bool CanHiring() => IsBuilded(_pull.GagaHouses) && IsBuilded(_pull.Cleaners) && IsBuilded(_pull.Storages);

        private bool IsBuilded(BuildTrigger[] buildings) =>
                buildings.Any(building => building.GetBuildMenu().IsBuilded);

        private void SetCell(EmployeeCell cell, Employee employee) =>
                cell.SetCell(employee, GlobalConstants.PersonalSpawnPoint, _playerInventory);
    }
}