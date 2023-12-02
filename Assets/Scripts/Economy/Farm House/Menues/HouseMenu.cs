using UnityEngine;

namespace Economy.Farm_House
{
    public class HouseMenu : MonoBehaviour
    {
        [SerializeField] private ShopMenu _shopMenu;
        [SerializeField] private TaskHandler _taskHandler;

        private void Awake() => Initialize();
        
        private void Initialize()
        {
            _shopMenu.Initialize();
            _taskHandler.Initialize();
        }
        
        public void SetInventory(Inventory inv)
        {
            _shopMenu.SetPlayerInventory(inv);
            _shopMenu.RefreshDisplay();
            
            _taskHandler.SetPlayerInventory(inv);
            _taskHandler.RefreshDisplay();
        }
    }
}