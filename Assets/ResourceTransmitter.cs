using System;
using System.Collections;
using Economy;
using Economy.Items;
using UnityEngine;

public class ResourceTransmitter : MonoBehaviour
{
    public delegate IEnumerator CoroutineDelegate(ItemType typeFrom, Inventory inv, int fluff);
    public event CoroutineDelegate TransmitteEvent;

    [SerializeField] private ItemType _typeFromPlayer;
    [SerializeField] private ItemType _typeToPlayer;

    [SerializeField, Header("Сколько пуха передается от игрока")]
    private int _fluffCount;

    private BuildStorage _storage;
    private Inventory _characterInventory;
    

 

    private void Start() => _storage = GetComponent<BuildStorage>();

    private void CheckBag()
    {
        if (_characterInventory == null) return;

        _characterInventory.AddItems(_typeToPlayer, _storage.GetFluff());
        _storage.ResetFluff();
    }

    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<InputHandler>())
        {
            _characterInventory = collision.gameObject.GetComponent<Inventory>();

            CheckBag();
            if (_fluffCount != 0 && TransmitteEvent != null)
            {
                StartCoroutine(TransmitteEvent.Invoke(_typeFromPlayer, _characterInventory, _fluffCount));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_characterInventory == collision.GetComponent<Inventory>() && collision.gameObject.GetComponent<InputHandler>()) _characterInventory = null;
    }

}