using System;
using Economy.Items;
using UnityEngine;

namespace Economy
{
    [Serializable] public class ItemBunch
    {
        [SerializeField] private ItemType _type;
        [SerializeField] private int _count;
        
        public ItemBunch(ItemType type, int count = 0)
        {
            _type = type;
            _count = count;
        }

        public void AddItems(int value)
        {
            AddValue(IsValidValue(value) ? value : 0);
        }

        public void RemoveItems(int value)
        {
            if (!IsValidValue(value)) return;
            
            AddValue(_count - value >= 0 ? -value : -_count);
        }
        
        public ItemType GetItemType() => _type;
        
        public int GetCount() => _count;

        private void AddValue(int value) => _count += value;
        
        private bool IsValidValue(int value) => value > 0 ;
    }
}