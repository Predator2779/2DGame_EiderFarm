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
        [FormerlySerializedAs("_isPlayerInvemtory")] [SerializeField]
        private bool _isPlayerInventory;

        [SerializeField] private List<ItemBunch> _listItems;

        public void AddItems(ItemType type, int count) => AddOrCreate(type, count);

        public void RemoveItems(ItemType type, int count)
        {
            if (!IsExistsItems(type, count)) return;

            Remove(type, count);
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

            if (TryGetBunch(type, out ItemBunch newBunch))
            {
                bunch = newBunch;
            }
            else
            {
                _listItems.Add(bunch);
            }

            bunch.AddItems(count);
            SendMessage(type, bunch.GetCount());
        }

        private void Remove(ItemType type, int count)
        {
            if (!TryGetBunch(type, out ItemBunch itemBunch)) return;

            itemBunch.RemoveItems(count);
            SendMessage(type, itemBunch.GetCount());
        }

        private void SendMessage(ItemType type, int count)
        {
            if (_isPlayerInventory) EventHandler.OnInventoryAdd?.Invoke(type, count);
        }

        #region Removed

        // public void Give(ref Inventory inv, ItemType type, int count)
        // {
        //     if (!IsExistsItems(type, count)) return;
        //
        //     inv.Add(type, count);
        //     RemoveItems(type, count);
        // }

        #endregion
    }
}