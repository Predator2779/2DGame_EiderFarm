using System.Collections.Generic;
using System.Linq;
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
                SendMessage(bunch.GetItemName(), bunch.GetCount());
        }

        public void AddItems(Item item, int count) => AddOrCreate(item, count);

        public void RemoveItems(Item item, int count)
        {
            if (IsExistsItems(item, count)) Remove(item, count);
        }

        private bool IsExistsItems(Item type, int count) =>
                _listItems.Any(bunch => bunch.GetItemName() ==
                        type.GetName() && bunch.GetCount() > count);

        private bool TryGetBunch(Item item, out ItemBunch itemBunch)
        {
            foreach (var bunch in _listItems.Where(bunch => bunch.GetItemName() == item.GetName()))
            {
                itemBunch = bunch;
                return true;
            }

            itemBunch = null;
            return false;
        }

        private void AddOrCreate(Item item, int count)
        {
            ItemBunch bunch = new ItemBunch(item);

            if (TryGetBunch(item, out ItemBunch newBunch))
            {
                bunch = newBunch;
            }
            else
            {
                _listItems.Add(bunch);
            }

            bunch.AddItems(count);
            SendMessage(item.GetName(), bunch.GetCount());
        }

        private void Remove(Item item, int count)
        {
            if (!TryGetBunch(item, out ItemBunch itemBunch)) return;

            itemBunch.RemoveItems(count);
            SendMessage(item.GetName(), itemBunch.GetCount());
        }

        private void SendMessage(string name, int count)
        {
            if (_isPlayerInventory) EventHandler.OnInventoryAdd?.Invoke(name, count);
        }
    }
}