using UnityEngine;

namespace Economy.Farm_House
{
    public class FHouseMenu : MonoBehaviour
    {
        [SerializeField] private ShopMenu _shopMenu;
        [SerializeField] private TaskHandler _taskHandler;

        public void SetInventory(Inventory inv)
        {
            _taskHandler.SetPlayerInventory(inv);
            _shopMenu.SetPlayerInventory(inv);
            _shopMenu.Draw();
        }
    }
}