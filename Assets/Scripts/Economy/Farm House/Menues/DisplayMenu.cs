using TMPro;
using UnityEngine;

namespace Economy.Farm_House
{
    public abstract class DisplayMenu : MonoBehaviour
    {
        [SerializeField] protected Transform _content;
        [SerializeField] private TMP_Text _btnText;

        protected Inventory _playerInventory;
        protected bool _isHouseMenu = true;

        public void SetPlayerInventory(Inventory inventory) => _playerInventory = inventory;

        public void SwitchDisplay()
        {
            _isHouseMenu = !_isHouseMenu;
            RefreshDisplay();
        }

        protected abstract void Draw();

        public void RefreshDisplay()
        {
            ClearContent();
            Draw();
        }

        private void ClearContent()
        {
            var length = _content.childCount;

            for (int i = 0; i < length; i++)
                Destroy(_content.GetChild(i).gameObject);
        }

        protected void SetBtnText(string text) => _btnText.text = text;
    }
}