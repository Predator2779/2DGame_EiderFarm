using UnityEngine;

namespace Economy.Farm_House
{
    [RequireComponent(typeof(Inventory))]
    public class ShopMenu : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private ShopCell _shopCellPrefab;

        private Inventory _playerInventory;
        private Inventory _assortment;
        private bool _isShopDisplayed = true;

        private void Awake() => Initialize();

        private void Initialize() => _assortment = GetComponent<Inventory>();

        public void SetPlayerInventory(Inventory inv) => _playerInventory = inv;

        public void SwitchDisplay()
        {
            _isShopDisplayed = !_isShopDisplayed;

            Draw();
        }

        public void Draw()
        {
            ClearContent();

            if (_isShopDisplayed)
            {
                DrawInventory(_assortment);
                return;
            }

            DrawInventory(_playerInventory);
        }

        private void ClearContent()
        {
            var length = _content.childCount;

            for (int i = 0; i < length; i++)
                Destroy(_content.GetChild(i).gameObject);
        }

        private void DrawInventory(Inventory inv)
        {
            var listBunch = inv.GetAllItems();

            foreach (var bunch in listBunch)
                SetCell(Instantiate(_shopCellPrefab, _content), bunch);
        }

        private void SetCell(ShopCell cell, ItemBunch bunch)
        {
            switch (_isShopDisplayed)
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