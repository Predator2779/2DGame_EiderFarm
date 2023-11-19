using UnityEngine;

namespace Economy.Farm_House
{
    [RequireComponent(typeof(Inventory))]
    public class ShopMenu : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private ShopCell _shopCellPrefab;

        [SerializeField] private Inventory _playerInventory;//hide
        private bool _isShopDisplayed = true;
        private Inventory _assortment;

        private void Awake() => Initialize();

        private void Initialize()
        {
            _assortment = GetComponent<Inventory>();
        }

        private void OnEnable()
        {
            DrawInventory(_assortment);
        }

        public void SetPlayerInventory(Inventory inv) => _playerInventory = inv;

        public void SwitchDisplay() => Draw();

        private void Draw()
        {
            _isShopDisplayed = !_isShopDisplayed;

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
            var list = inv.GetAllItems();

            foreach (var item in list)
                Instantiate(_shopCellPrefab, _content).
                        SetCell(item.GetItemIcon(), item.GetItemName(), item.GetCount());
        }
    }
}