using System.Collections;
using Building.Constructions;
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

    private Inventory _characterInventory;
    private Construction _construction;
    private Inventory _inventory;

    private void Start()
    {
        _construction = GetComponent<Construction>();
        _inventory = GetComponent<Inventory>();
    }

    public void CheckBag()
    {
        if (_characterInventory == null) return;

        Transmitte();

        if (_fluffCount != 0 && TransmitteEvent != null)
            StartCoroutine(TransmitteEvent?.Invoke(_typeToPlayer, _characterInventory, _fluffCount));
    }

    private void Transmitte()
    {
        var resources = _inventory.GetAllItems();
        
        if (resources == null) return;
        
        _characterInventory.AddItemsWithMsg(resources.ToArray(), _construction);
        _inventory.ResetInventory();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<InputHandler>())
        {
            _characterInventory = collision.gameObject.GetComponent<Inventory>();
            CheckBag();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_characterInventory == collision.GetComponent<Inventory>() &&
            collision.gameObject.GetComponent<InputHandler>())
            _characterInventory = null;
    }
}