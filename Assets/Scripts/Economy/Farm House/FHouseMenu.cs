using UnityEngine;

namespace Economy.Farm_House
{
    public class FHouseMenu : MonoBehaviour
    {
        [SerializeField] private ShopMenu _shopMenu;

        public void SetInventory(Inventory inv)
        {
            _shopMenu.SetPlayerInventory(inv);
            _shopMenu.Draw();
        }
    }
}