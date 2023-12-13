using System.Linq;
using General;
using UnityEngine;

namespace Economy.Farm_House.Menues
{
    [RequireComponent(typeof(Inventory))]
    public class ShopMenu : DisplayMenu
    {
        [SerializeField] private ShopCell _shopCellPrefab;
        
        private Inventory _assortment;

        public void Initialize()
        {
            _prefabText = _shopCellPrefab.GetText();
            _assortment = GetComponent<Inventory>();
        }
        
        protected override void Draw()
        {
            if (_isHouseMenu)
            {
                SetBtnText("Магазин");
                DrawInventory(_assortment);
                return;
            }

            SetBtnText("Инвентарь");
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