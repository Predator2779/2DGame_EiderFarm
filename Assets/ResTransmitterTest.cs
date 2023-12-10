using System.Collections;
using Building.Constructions;
using Characters;
using Economy;
using General;
using UnityEngine;

[RequireComponent(typeof(Construction))]
[RequireComponent(typeof(BuildStorage))]
public class ResTransmitterTest : MonoBehaviour
{
    public delegate IEnumerator CoroutineDelegate(Item typeFrom, Inventory inv, int fluff);

    public event CoroutineDelegate TransmitteEvent;

    [SerializeField, Header("Сколько пуха передается от игрока")]
    private int _fluffCount;

    [SerializeField] private Item _typeToPlayer;
    [SerializeField] private Inventory _characterInventory;
    private Construction _construction;
    private BuildStorage _storage;

    private void Start()
    {
        _construction = GetComponent<Construction>();
        _storage = GetComponent<BuildStorage>();
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
            _characterInventory.AddItems(_typeToPlayer, _storage.GetFluffCount());
            _storage.ResetFluff();
        }
        else
        {
            if (!_characterInventory.TryGetBunch(GlobalConstants.CleanedFluff, out ItemBunch bunch))
                return;

            _storage.AddFluff(bunch.GetCount());
            _characterInventory.RemoveItems(bunch.GetItem(), bunch.GetCount());
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