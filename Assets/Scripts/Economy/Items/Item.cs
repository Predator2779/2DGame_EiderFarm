using System;
using UnityEngine;

namespace Economy
{
    [CreateAssetMenu(menuName = "Inventory/Item", fileName = "New Item", order = 0)]
    [Serializable] public class Item : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private int _price;

        [SerializeField] private Sprite[] _sprites;

        [SerializeField] private bool _isOneAtTime;

        public Sprite GetIcon() => _icon;

        public Sprite[] GetSprites() => _sprites;
        public string GetName() => _name;
        public int GetPrice() => _price;

        public bool IsOne() => _isOneAtTime;
    }
}