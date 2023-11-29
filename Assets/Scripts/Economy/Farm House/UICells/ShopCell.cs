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
        private Inventory _invFrom;
        private Inventory _invTo;

        public void SetCell(ItemBunch bunch, Inventory invFrom, Inventory invTo)
        {
            _bunch = bunch;
            _invFrom = invFrom;
            _invTo = invTo;

            RefreshButton();
        }

        private void RefreshButton() =>
                SetButton(_bunch.GetItemIcon(),
                        _bunch.GetItemName(),
                        _bunch.GetCount());

        public void Exchange() // переписать
        {
            Item item = _bunch.GetItem();

            if (item.IsOne() && _invTo.GetAllItems()[4].GetCount() != 0) return;
            if (item.IsOne() && _invTo.GetAllItems()[4].GetCount() == 0) _inpField.text = 1.ToString();

            int count = GetCountFromInput();

            if (count > _bunch.GetCount()) return;

            if (!(_invTo.TryGetBunch(GlobalConstants.Money, out ItemBunch bunchTo) &&
                _invFrom.TryGetBunch(GlobalConstants.Money, out ItemBunch bunchFrom) &&
                IsEnoughMoney(_invTo, count))) return;
            
            RemoveMoney(bunchTo, count);
            AddMoney(bunchFrom, count);

            _invFrom.RemoveItems(item, count);
            _invTo.AddItems(item, count);

            RefreshButton();
            CheckCount();
        }
        
        private bool IsEnoughMoney(Inventory inv, int count) =>
                inv.IsExistsItems(GlobalConstants.Money, count);

        private void AddMoney(ItemBunch wallet, int countItems)
        {
            var item = _bunch.GetItem();
            wallet.AddItems(item.GetPrice() * countItems);
        }
        
        private void RemoveMoney(ItemBunch wallet, int countItems)
        {
            var item = _bunch.GetItem();
            wallet.RemoveItems(item.GetPrice() * countItems);
        }

        private void CheckCount()
        {
            if (_bunch.GetCount() <= 0) Destroy(gameObject);
        }

        private int GetCountFromInput()
        {
            string text = _inpField.text;
            
            if (text == "") return 1;
            
            int count = Convert.ToInt32(text);
            
            return count > 0 ? count : 0;
        }
    }
}