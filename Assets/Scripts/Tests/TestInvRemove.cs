using Economy;
using Economy.Items;
using UnityEngine;

namespace Tests
{
    public class TestInvRemove : MonoBehaviour
    {
        public ItemType type;
        public int count;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Inventory _inventory))
                _inventory.RemoveItems(type, count);
        }
    }
}