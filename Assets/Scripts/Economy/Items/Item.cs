using System;
using UnityEngine;

namespace Economy
{
    [CreateAssetMenu(menuName = "Inventory/Item", fileName = "New Item", order = 0)]
    [Serializable] public class Item : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private int _price;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _isOneAtTime;

        private Sprite _currentSprite;

        public Sprite GetIcon() => _icon;
        public Sprite[] GetSprites() => _sprites;
        public Sprite GetCurrentSprite() => _currentSprite;
        public string GetName() => _name;
        public string GetDescription() => _description;
        public int GetPrice() => _price;
        public string GetDescriptiption() => _description;
        public bool IsOne() => _isOneAtTime;
    }
}