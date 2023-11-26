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

        public void Exchange()
        {
            Item item = _bunch.GetItem();
            int count = GetCountFromInput();

            if (count > _bunch.GetCount()) return;

            if (!_invTo.TryGetBunch(GlobalConstants.Money, out ItemBunch bunchTo) ||
                !_invFrom.TryGetBunch(GlobalConstants.Money, out ItemBunch bunchFrom) ||
                !IsEnoughMoney(_invTo, count)) return;

            /// убрать проверку кошелька на добавление денег

            Sell(bunchTo, count);
            Buy(bunchFrom, count);

            _invFrom.RemoveItems(item, count);
            _invTo.AddItems(item, count);

            RefreshButton();
            CheckCount();
        }

        private bool IsEnoughMoney(Inventory inv, int count) =>
                inv.IsExistsItems(GlobalConstants.Money, count);

        private void Sell(ItemBunch wallet, int countItems)
        {
            var item = _bunch.GetItem();
            wallet.RemoveItems(item.GetPrice() * countItems);
        }

        private void Buy(ItemBunch wallet, int countItems)
        {
            var item = _bunch.GetItem();
            wallet.AddItems(item.GetPrice() * countItems);
        }

        private void CheckCount()
        {
            if (_bunch.GetCount() <= 0) Destroy(gameObject);
        }

        private int GetCountFromInput() /// запрашиваемое кол-во из inputField
        {
            int count = Convert.ToInt32(_inpField.text);

            return count > 0 ? count : 0;
        }
    }
}