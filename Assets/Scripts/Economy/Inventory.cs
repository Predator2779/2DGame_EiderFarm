using System.Collections.Generic;
using System.Linq;
using Economy.Items;
using General;
using UnityEngine;
using UnityEngine.Serialization;

namespace Economy
{
    public class Inventory : MonoBehaviour
    {
        [FormerlySerializedAs("_isPlayerInvemtory")] [SerializeField] private bool _isPlayerInventory;
        [SerializeField] private List<ItemBunch> _listItems;

        public void Add(ItemType type, int count) => AddOrCreate(type, count);

        public void Give(ref Inventory inv, ItemType type, int count)
        {
            if (!IsExistsItems(type, count)) return;
            
            inv.Add(type, count);
            RemoveItems(type, count);
        }

        public void GiveToStorage(ref BuildStorage storage, ItemType item, int count)
        {
            if (!IsExistsItems(item, count)) return;
            
            storage.AddFluff(count);
            RemoveItems(item, count);
        }
        
        private bool IsExistsItems(ItemType type, int count) =>
                _listItems.Any(bunch => bunch.GetItemType() == 
                        type && bunch.GetCount() == count);
        
        private bool TryGetBunch(ItemType type, out ItemBunch itemBunch)
        {
            foreach (var bunch in _listItems.Where(bunch => bunch.GetItemType() == type))
            {
                itemBunch = bunch;
                return true;
            }

            itemBunch = null;
            return false;
        }

        private void AddOrCreate(ItemType type, int count)
        {
            ItemBunch bunch = new ItemBunch(type);
            
            if (TryGetBunch(type, out bunch))
            {
                bunch.AddItems(count);
                return;
            }

            bunch.AddItems(count);
            _listItems.Add(bunch);
            SendMessage(type, bunch.GetCount());
        }

        private void RemoveItems(ItemType type, int count)
        {
            if (!TryGetBunch(type, out ItemBunch itemBunch)) return;
            
            itemBunch.RemoveItems(count);
            SendMessage(type, itemBunch.GetCount());
        }
        
        private void SendMessage(ItemType type, int count)
        {
            if (_isPlayerInventory) EventHandler.OnInventoryAdd?.Invoke(type, count);
        }
    }
}