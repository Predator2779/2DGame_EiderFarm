using System.Collections.Generic;
using System.Linq;
using Building.Constructions;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Economy
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private bool _isPlayerInventory;
        [SerializeField] private List<ItemBunch> _listItems;
        [SerializeField] private int _limit;

        private void Start()
        {
            foreach (var bunch in _listItems)
                SendCountItemsMsg(bunch.GetItemName(), bunch.GetCount());
        }

        public void AddItems(Item item, int count) => AddOrCreate(item, count);

        public void AddItems(ItemBunch[] bunches)
        {
            foreach (var bunch in bunches)
                AddOrCreate(bunch.GetItem(), bunch.GetCount());
        }

        public void AddItemsWithMsg(ItemBunch[] bunches, Construction construction)
        {
            foreach (var bunch in bunches)
            {
                Item item = bunch.GetItem();
                int count = bunch.GetCount();

                AddOrCreate(item, count);
                SendTransmitteMsg(construction, item, count);
            }
        }

        public void RemoveItems(Item item, int count)
        {
            if (IsExistsItems(item.GetName(), count)) Remove(item, count);
        }

        public void Exchange(Inventory invFrom, Inventory invTo, ItemBunch bunchFrom)
        {
            if (invFrom.IsExistsItems(bunchFrom.GetItem().GetName(), bunchFrom.GetCount()))
                return;

            int count = invTo.GetFreeSpace();

            invTo.AddItems(bunchFrom.GetItem(), count);
            invFrom.Remove(bunchFrom.GetItem(), count);
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
        
        public int GetFreeSpace() => _limit - GetTotalCount();
        private int GetTotalCount() => _listItems == null ? 0 : _listItems.Sum(bunch => bunch.GetCount());
        public List<ItemBunch> GetAllItems() => _listItems;
        public void ReplaceAllBunches(List<ItemBunch> bunches) => _listItems = bunches;

        public bool IsExistsItems(string name, int count)
        {
            if (count < 0) return false;

            return _listItems.Any(bunch => bunch.GetItemName() ==
                    name && bunch.GetCount() >= count);
        }

        private void AddOrCreate(Item item, int count)
        {
            var bunch = GetBunch(item);

            AddUpToLimit(bunch, count);
            SendCountItemsMsg(item.GetName(), bunch.GetCount());
            SendCountAddedMsg(item, count);
        }

        private void AddUpToLimit(ItemBunch bunch, int count)
        {
            int free = _limit - GetTotalCount();

            if (count <= free || _limit == 0)
                bunch.AddItems(count);
            else bunch.AddItems(free);
        }

        private ItemBunch GetBunch(Item item)
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

        public void ResetInventory() => _listItems = null;

        private void SendCountItemsMsg(string name, int count)
        {
            if (!_isPlayerInventory) return;

            EventHandler.OnBunchChanged?.Invoke(name, count);

            if (TryGetBunch("Флажок", out ItemBunch bunch))
                EventHandler.OnFlagChanged?.Invoke(bunch.GetCount(), bunch.GetItem().GetSprites());
        }

        private void SendCountAddedMsg(Item item, int count)
        {
            if (_isPlayerInventory) EventHandler.OnItemPickUp?.Invoke(item, count);
        }

        private void SendTransmitteMsg(Construction construction, Item item, int count)
        {
            if (_isPlayerInventory) EventHandler.OnItemTransmitted?.Invoke(construction.typeConstruction, item, count);
        }

        private void SendCountRemovedMsg(Item item, int count)
        {
            if (_isPlayerInventory) EventHandler.OnItemPut?.Invoke(item, count);
        }
    }
}