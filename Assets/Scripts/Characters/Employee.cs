using Economy;
using General;
using UnityEngine;

namespace Characters
{
    public class Employee : Person
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _description;
        [SerializeField] private int _price;
        [SerializeField] private bool _isHired;
        [SerializeField] private Inventory _inventory;

        public Sprite GetSprite() => _sprite;
        public string GetDescription() => _description;
        public int GetPrice() => _price;
        public bool CanHiring() => !_isHired;

        public void Hire(Inventory inventory, Vector2 spawnPosition)
        {
            if (!inventory.TryGetBunch(GlobalConstants.Money, out ItemBunch wallet)) return;
            
            Item money = wallet.GetItem();
            inventory.RemoveItems(money, _price);
            _inventory.AddItems(money, _price);
            
            SpawnEmployee(spawnPosition);
        }

        private void SpawnEmployee(Vector2 spawnPosition)
        {
            Instantiate(this, spawnPosition, Quaternion.identity);
        }
    }
}