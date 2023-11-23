using System.Linq;
using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [RequireComponent(typeof(Inventory))]
    public class ShopMenu : DisplayMenu
    {
        [SerializeField] private ShopCell _shopCellPrefab;

        private Inventory _playerInventory;
        private Inventory _assortment;

        private void Awake() => Initialize();

        private void Initialize() => _assortment = GetComponent<Inventory>();

        public void SetPlayerInventory(Inventory inv) => _playerInventory = inv;
        
        public override void Draw()
        {
            ClearContent();

            if (_isHouseMenu)
            {
                DrawInventory(_assortment);
                return;
            }

            DrawInventory(_playerInventory);
        }

        private void DrawInventory(Inventory inv)
        {
            var listBunch = inv.GetAllItems();

            foreach (var bunch in listBunch.Where(bunch => bunch.GetItemName() != GlobalConstants.Money))
                SetCell(Instantiate(_shopCellPrefab, _content), bunch);
        }

        private void SetCell(ShopCell cell, ItemBunch bunch)
        {
            switch (_isHouseMenu)
            {
                case true:
                    cell.SetCell(bunch, _assortment, _playerInventory);
                    break;
                case false:
                    cell.SetCell(bunch, _playerInventory, _assortment);
                    break;
            }
        }
    }
}