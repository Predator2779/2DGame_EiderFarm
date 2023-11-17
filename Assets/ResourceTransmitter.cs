using Economy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTransmitter : MonoBehaviour
{
    [SerializeField] private BagContent _typeFromPlayer;
    [SerializeField] private Converter _converter;
    private BagContent _typeToPlayer;
    private BuildStorage _storage;
    private Bag _characterBag;
    private Bag _characterSecondBag;

    private int _fluff;
    private void Start() => _storage = GetComponent<BuildStorage>();

    private void CheckBag()
    {
        if (_characterBag == null) return;

        _fluff = _storage.GetFluff();
        if (_characterSecondBag != null)
        {
            _characterSecondBag.AddPoints(-_fluff);
            Debug.Log(1111);
        }
            _characterBag.AddPoints(_fluff > 0 ? _fluff : 0);
        _storage.ResetFluff();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Inventory inventory = collision.gameObject.GetComponent<Inventory>();
        if (_converter == null)
        {
            _typeToPlayer = BagContent.UncleanedFluff;
        }
        else
        {
            _typeToPlayer = _converter.Convert(_typeFromPlayer);
            inventory.
                      TryGetBag(_typeFromPlayer, out _characterSecondBag);
            _fluff = _characterSecondBag.GetCurrentPoints();
            _storage.AddFluff(_fluff);
        }
        inventory.
                  TryGetBag(_typeToPlayer, out _characterBag);

        CheckBag();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Inventory>().
                  TryGetBag(_typeFromPlayer, out Bag bag);

        if (_characterBag == bag) _characterBag = null;
    }

}
