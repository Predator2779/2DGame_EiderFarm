using Economy;
using Economy.Farm_House;
using UnityEngine;

namespace TriggerScripts
{
    public class HouseTrigger : MenuTrigger
    {
        [SerializeField] private HouseMenu _houseMenu;
        [SerializeField] private CreateBuildingTask _createBuildingTask;
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);

            if (other.TryGetComponent(out Inventory inv))
                _houseMenu.SetInventory(inv);
        }
    }
}