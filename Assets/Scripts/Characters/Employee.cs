using Economy;
using General;
using UnityEngine;

namespace Characters
{
    public class Employee : Person
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private int _price;
        [SerializeField] private bool _isHired;
        [SerializeField] private Inventory _inventory;

        public Sprite GetSprite() => _sprite;
        public string GetName() => _name;
        public string GetDescription() => _description;
        public int GetPrice() => _price;
        public bool CanHiring() => !_isHired;

        public void Hire(Inventory inventory)
        {
            if (!inventory.TryGetBunch(GlobalConstants.Money, out ItemBunch wallet) ||
                wallet.GetCount() < _price) return;
            
            Item money = wallet.GetItem();
            inventory.RemoveItems(money, _price);
            _inventory.AddItems(money, _price);
        }
    }
}