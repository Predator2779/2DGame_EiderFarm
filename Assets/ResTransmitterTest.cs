using System.Collections;
using Building.Constructions;
using Characters;
using Economy;
using UnityEngine;

[RequireComponent(typeof(Construction))]
[RequireComponent(typeof(Inventory))]
public class ResTransmitterTest : MonoBehaviour
{
    public delegate IEnumerator CoroutineDelegate(Item typeFrom, Inventory inv, int fluff);
    public event CoroutineDelegate TransmitteEvent;

    [SerializeField, Header("Сколько пуха передается от игрока")] private int _fluffCount;
    [SerializeField] private Item _typeToPlayer;
    [SerializeField] private Inventory _characterInventory;
    private Construction _construction;
    private Inventory _inventory;

    private void Start()
    {
        _construction = GetComponent<Construction>();
        _inventory = GetComponent<Inventory>();
    }

    private void CheckBag()
    {
        if (_characterInventory == null) return;

        Transmitte();

        if (_fluffCount != 0 && TransmitteEvent != null)
            StartCoroutine(TransmitteEvent?.Invoke(_typeToPlayer, _characterInventory, _fluffCount));
    }

    private void Transmitte()
    {
        if (_characterInventory.IsPlayerInventory())
        {
            var resources = _inventory.GetAllItems();
        
            if (resources == null) return;
        
            _characterInventory.AddItemsWithMsg(resources.ToArray(), _construction);
            _inventory.ResetInventory();
        }
        else
        {
            var resources = _characterInventory.GetAllItems();
        
            if (resources == null) return;
        
            _inventory.AddItems(resources.ToArray());
            _characterInventory.ResetInventory();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Person>())
        {
            _characterInventory = collision.gameObject.GetComponent<Inventory>();
            CheckBag();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_characterInventory == collision.GetComponent<Inventory>() &&
            collision.gameObject.GetComponent<Person>())
            _characterInventory = null;
    }
}