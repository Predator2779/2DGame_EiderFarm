using UnityEngine;

namespace Economy.Farm_House
{
    public class HouseMenu : MonoBehaviour
    {
        [SerializeField] private ShopMenu _shopMenu;
        [SerializeField] private TaskHandler _taskHandler;

        public void SetInventory(Inventory inv)
        {
            _shopMenu.SetPlayerInventory(inv);
            _shopMenu.RefreshDisplay();
            
            _taskHandler.SetPlayerInventory(inv);
            _taskHandler.RefreshDisplay();
        }
    }
}