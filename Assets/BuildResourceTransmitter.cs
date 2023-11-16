using Economy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildResourceTransmitter : MonoBehaviour
{
    [SerializeField] private BuildStorage _storage;

    private int fluffCount;

    private void Start() => _storage = GetComponentInParent<BuildStorage>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<InputHandler>())
        {
            fluffCount = _storage.GetFluffFromStorage();
            if(fluffCount > 0)
            {
                collision.gameObject.GetComponent<Inventory>().TryGetBag(BagContent.UncleanedFluff, out Bag bag);
                bag.AddPoints(fluffCount);
                _storage.ResetFluff();
            }
        }
    }
}
