using System;
using UnityEngine;

namespace Economy.Farm_House
{
    public abstract class DisplayMenu : MonoBehaviour
    {
        [SerializeField] protected Transform _content;

        [SerializeField] protected Inventory _playerInventory;
        protected bool _isHouseMenu = true; // отображается меню сущностей инвентаря игрока или фермерского домика.

        public void SetPlayerInventory(Inventory inventory) => _playerInventory = inventory;

        public void DisplayActive(bool value)
        {
            _isHouseMenu = value;
            Draw();
        }

        public abstract void Draw();

        public void RefreshDisplay()
        {
            ClearContent();
            Draw();
        }
        
        protected void ClearContent()
        {
            var length = _content.childCount;

            for (int i = 0; i < length; i++)
                Destroy(_content.GetChild(i).gameObject);
        }
    }
}