using System.Collections.Generic;
using System.Linq;
using General;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Economy
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private bool _isPlayerInventory;
        [SerializeField] private List<ItemBunch> _listItems;

        private void Start()
        {
            foreach (var bunch in _listItems)
                SendCountItemsMsg(bunch.GetItemName(), bunch.GetCount());
        }

        public void AddItems(Item item, int count) => AddOrCreate(item, count);

        public void RemoveItems(Item item, int count)
        {
            if (IsExistsItems(item.GetName(), count)) Remove(item, count);
        }

        public List<ItemBunch> GetAllItems() => _listItems;

        public bool IsExistsItems(string name, int count)
        {
            if (count < 0) return false;
            
            return _listItems.Any(bunch => bunch.GetItemName() ==
                    name && bunch.GetCount() >= count);
        }

        public bool TryGetBunch(string name, out ItemBunch itemBunch)
        {
            foreach (var bunch in _listItems.Where(bunch => bunch.GetItemName() == name))
            {
                itemBunch = bunch;
                return true;
            }

            itemBunch = null;
            return false;
        }

        private void AddOrCreate(Item item, int count)
        {
            var bunch = GetBunch(item);
            
            bunch.AddItems(count);
            SendCountItemsMsg(item.GetName(), bunch.GetCount());
            SendCountAddedMsg(item, count);
        }

        private ItemBunch GetBunch(Item item)//
        {
            ItemBunch bunch = new ItemBunch(item);

            if (TryGetBunch(item.GetName(), out ItemBunch newBunch))
                bunch = newBunch;
            else
                _listItems.Add(bunch);

            return bunch;
        }

        private void Remove(Item item, int count)
        {
            if (!TryGetBunch(item.GetName(), out ItemBunch bunch)) return;

            bunch.RemoveItems(count);
            CheckCount(bunch);
            SendCountItemsMsg(item.GetName(), bunch.GetCount());
            SendCountRemovedMsg(item, count);
        }

        private void CheckCount(ItemBunch bunch)
        {
            if (bunch.GetCount() <= 0) _listItems.Remove(bunch);
        }

        private void SendCountItemsMsg(string name, int count)
        {
            if (!_isPlayerInventory) return;

            EventHandler.OnBunchChanged?.Invoke(name, count);
            EventHandler.OnFlagChanged?.Invoke(GetAllItems()[4].GetCount(), GetAllItems()[4].GetItem().GetSprites());
        }

        private void SendCountAddedMsg(Item item, int count)
        {
            if (_isPlayerInventory) EventHandler.OnItemPickUp?.Invoke(item, count);
        }

        private void SendCountRemovedMsg(Item item, int count)
        {
            if (_isPlayerInventory) EventHandler.OnItemPut?.Invoke(item, count);
        }
    }
}