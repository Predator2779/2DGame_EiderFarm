using System;
using TMPro;
using UnityEngine;

namespace Economy.Farm_House
{
    public class ShopCell : MenuCell
    {
        [SerializeField] private TMP_InputField _inpField;
        
        [SerializeField] private ItemBunch _bunch;
        private Inventory _invFrom;
        [SerializeField] private Inventory _invTo;
        
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
            int count = GetCountFromInput();
            
            if (count > _bunch.GetCount()) return;
            
            _invFrom.RemoveItems(_bunch.GetItem(), count);
            _invTo.AddItems(_bunch.GetItem(), count);
            
            RefreshButton();
            CheckCount();
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