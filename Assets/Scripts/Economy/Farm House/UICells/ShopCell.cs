using System;
using General;
using TMPro;
using UnityEngine;

namespace Economy.Farm_House
{
    public class ShopCell : MenuCell
    {
        [SerializeField] private TMP_InputField _inpField;

        private ItemBunch _bunch;
        private Inventory _seller;
        private Inventory _buyer;
        private ItemBunch _sellerWallet;
        private ItemBunch _buyerWallet;
        
        public void SetCell(ItemBunch bunch, Inventory invFrom, Inventory invTo)
        {
            _bunch = bunch;
            _seller = invFrom;
            _buyer = invTo;

            RefreshButton();
        }

        public void Exchange()
        {
            Item item = _bunch.GetItem();

            int count = GetCountFromInput();
            var price = item.GetPrice() * count;
            
            if (_buyer.TryGetBunch("Ôëàæîê", out ItemBunch bunch))
            {
                if (item.IsOne() && bunch.GetCount() != 0) return;
                if (item.IsOne() && bunch.GetCount() == 0) count = 1;
            }
            
            if (count > _bunch.GetCount()) return;
            if (!CheckWallets(_buyer, _seller)) return;;
            if (!IsEnoughMoney(_buyer, price)) return;
            
            
            Sell(item, count, price);
            Buy(item, count, price);

            RefreshButton();
            CheckCount();
        }

        private void RefreshButton() =>
                SetButton(_bunch.GetItemIcon(),
                        _bunch.GetItemName(),
                        _bunch.GetCount());
        
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

        private int GetCountFromInput()
        {
            string text;
            if (_inpField != null)
            {
                text = _inpField.text;
            }
            else text = 1.ToString();
            
            
            if (text == "") return 1;
            
            int count = Convert.ToInt32(text);
            
            return count > 0 ? count : 0;
        }
    }
}