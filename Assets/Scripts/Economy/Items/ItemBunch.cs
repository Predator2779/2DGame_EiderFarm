using System;
using UnityEngine;

namespace Economy
{
    [Serializable] public class ItemBunch
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _count;

        public ItemBunch(Item item, int count = 0)
        {
            _item = item;
            _count = count;
        }

        public void AddItems(int value) => AddValue(IsValidValue(value) ? value : 0);

        public void RemoveItems(int value)
        {
            if (!IsValidValue(value)) return;

            AddValue(_count - value >= 0 ? -value : -_count);
        }
        
        public Item GetItem() => _item;
        
        public Sprite GetItemIcon() => _item.GetIcon();
        
        public string GetItemName() => _item.GetName();

        public int GetCount() => _count;

        private void AddValue(int value) => _count += value;

        private bool IsValidValue(int value) => value > 0;
    }
}