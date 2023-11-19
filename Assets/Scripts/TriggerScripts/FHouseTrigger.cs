using Economy;
using Economy.Farm_House;
using UnityEngine;

namespace TriggerScripts
{
    public class FHouseTrigger : MenuTrigger
    {
        [SerializeField] private FHouseMenu _fHouseMenu;
        
        protected void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);

            if (other.TryGetComponent(out Inventory inv))
                _fHouseMenu.SetInventory(inv);
        }
    }
}