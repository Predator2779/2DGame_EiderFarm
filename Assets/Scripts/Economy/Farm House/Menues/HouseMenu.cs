using UnityEngine;

namespace Economy.Farm_House
{
    public class HouseMenu : MonoBehaviour
    {
        [SerializeField] private ShopMenu _shopMenu;
        [SerializeField] private TaskHandler _taskHandler;
        [SerializeField] private PersonalMenu _personalMenu;

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
            
            _personalMenu.SetPlayerInventory(inv);
            _personalMenu.RefreshDisplay();
        }
    }
}