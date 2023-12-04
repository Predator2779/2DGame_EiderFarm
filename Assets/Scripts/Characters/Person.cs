using UnityEngine;

namespace Characters
{
    public class Person : Animal
    {
        [Header("Character name.")]
        [SerializeField] protected string _name;
        public string GetName() => _name;
    }
}