using System;
using UnityEngine;

namespace Economy
{
    [CreateAssetMenu(menuName = "Inventory/Item", fileName = "New Item", order = 0)]
    [Serializable] public class Item : ScriptableObject
    {
        [SerializeField] private string _name;

        public string GetName() => _name;
    }
}