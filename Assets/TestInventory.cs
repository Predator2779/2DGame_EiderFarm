using Economy;
using UnityEngine;

public class TestInventory : MonoBehaviour
{
    [SerializeField] private BagContent _content;
    [SerializeField] private int _minValue;
    [SerializeField] private int _maxValue;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Inventory _inventory))
            if (_inventory.TryGetBag(BagContent.Money, out Bag bag))
                bag.AddPoints(Random.Range(_minValue, _maxValue));
    }
}