using Economy;
using UnityEngine;
using UnityEngine.Serialization;

public class TestInventoryAdd : MonoBehaviour
{
    [FormerlySerializedAs("type")] public Item item;
    public int count;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Inventory _inventory))
            _inventory.AddItems(item, count);
    }
}