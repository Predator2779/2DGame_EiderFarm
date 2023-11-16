using Economy;
using UnityEngine;

public class BuildStorage : MonoBehaviour
{
    [Header("Количество пуха.")]
    [SerializeField] private int _fluffCount;

    private Bag _characterBag;

    public void AddFluff()
    {
        _fluffCount++;

        CheckBag();
    }

    private void ResetFluff() => _fluffCount = 0;
    
    private void CheckBag()
    {
        if (_characterBag == null) return;

        _characterBag.AddPoints(_fluffCount > 0 ? _fluffCount : 0);
        ResetFluff();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Inventory>().
                  TryGetBag(BagContent.UncleanedFluff, out _characterBag);

        CheckBag();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Inventory>().
                  TryGetBag(BagContent.UncleanedFluff, out Bag bag);

        if (_characterBag == bag) _characterBag = null;
    }
}