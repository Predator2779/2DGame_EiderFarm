using Economy;
using UnityEngine;

public class TestInventory : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Inventory _inventory))
            if (_inventory.TryGetBag(BagContent.Money, out Bag bag))
                bag.AddPoints(Random.Range(10, 50));
    }
}