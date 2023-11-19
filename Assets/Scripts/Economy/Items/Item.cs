using System;

namespace Economy.Items
{
    [Serializable] public class Item
    {
        private readonly ItemType _type;
        public Item(ItemType type) => _type = type;
        public ItemType GetItemType() => _type;
    }

    public enum ItemType
    {
        Item,
        Money,
        CleanedFluff,
        UncleanedFluff
    }
}