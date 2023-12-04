using UnityEngine;

namespace Characters
{
    public class Person : Animal
    {
        [Header("Character name.")]
        [SerializeField] private string _name;
        public string GetName() => _name;
    }
}