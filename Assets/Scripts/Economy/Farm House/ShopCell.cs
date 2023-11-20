using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Economy.Farm_House
{
    public class ShopCell : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inpField;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _counter;

        private ItemBunch _bunch;
        private Inventory _invFrom;
        private Inventory _invTo;
        
        public void SetCell(ItemBunch bunch, Inventory invFrom, Inventory invTo)
        {
            _bunch = bunch;
            _invFrom = invFrom;
            _invTo = invTo;

            SetButton(bunch.GetItemIcon(),
                    bunch.GetItemName(),
                    bunch.GetCount());
        }

        private void SetButton(Sprite icon, string description, int count)
        {
            _icon.sprite = icon;
            _description.text = description;
            _counter.text = count.ToString();
        }

        private void RefreshButton() => 
                SetButton(_bunch.GetItemIcon(),
                _bunch.GetItemName(),
                _bunch.GetCount());
        
        public void Exchange()
        {
            int count = GetCountFromInput();
            
            // if (count <= 0) return;
            
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