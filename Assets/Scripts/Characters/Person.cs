using UnityEngine;

namespace Characters
{
    public class Person : Walker
    {
        [Header("Character name.")]
        [SerializeField] protected string _name;
        public string GetName() => _name;
    }
}