using General;
using TMPro;
using UnityEngine;

namespace Economy.Farm_House
{
    public class ShopCell : MenuCell
    {
        [SerializeField] private TextMeshProUGUI _prefabButtonText;

        private ItemBunch _bunch;
        private Inventory _seller;
        private Inventory _buyer;
        private ItemBunch _sellerWallet;
        private ItemBunch _buyerWallet;
        private Item _item;
        private int _count;
        private int _price;
        
        public void SetCell(ItemBunch bunch, Inventory invFrom, Inventory invTo)
        {
            _bunch = bunch;
            _seller = invFrom;
            _buyer = invTo;
            _item = _bunch.GetItem();
            _count = GetBuyCount(_seller, _bunch.GetCount());
            _price = _item.GetPrice() * _count;
            _priceField.text = _price.ToString();

            RefreshButton();
        }

        public TextMeshProUGUI GetText() => _prefabButtonText;

        public void Exchange()
        {
            if (_item.GetName() == GlobalConstants.Flag && 
                _buyer.IsPlayerInventory() &&
                _buyer.TryGetBunch(GlobalConstants.Flag, out ItemBunch bunch))
            {
                if (_item.IsOne() && bunch.GetCount() > 0) return;
                // if (_item.IsOne() && bunch.GetCount() == 0) count = 1;
            }
            
            if (!CheckWallets(_buyer, _seller)) return;;
            if (!IsEnoughMoney(_buyer, _price)) return;
            
            Sell(_item, _count, _price);
            Buy(_item, _count, _price);

            RefreshButton();
            CheckCount();
        }

        private int GetBuyCount(Inventory seller, int count) =>
                !seller.IsPlayerInventory() 
                || _item.GetName() == GlobalConstants.Flag 
                        ? 1 : count;

        private void RefreshButton() =>
                SetButton(_bunch.GetItemIcon(),
                        _bunch.GetItemName(),
                        _bunch.GetItemDescription(),
                        _count,
                        _price,
                        true);
                        
        
        private bool IsEnoughMoney(Inventory inv, int value) =>
                inv.IsExistsItems(GlobalConstants.Money, value);
        
        private void Buy(Item item, int count, int price)
        {
            _buyer.RemoveItems(_buyerWallet.GetItem(), price);
            _buyer.AddItems(item, count);
        }

        private void Sell(Item item, int count, int price)
        {
            _seller.AddItems(_sellerWallet.GetItem(), price);
            _seller.RemoveItems(item, count);
        }
        
        private bool CheckWallets(Inventory buyer, Inventory seller) =>
                buyer.TryGetBunch(GlobalConstants.Money, out _buyerWallet) &&
                seller.TryGetBunch(GlobalConstants.Money, out _sellerWallet);

        private void CheckCount()
        {
            if (_bunch.GetCount() <= 0) Destroy(gameObject);
        }
    }
}