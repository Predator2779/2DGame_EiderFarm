using System.Collections;
using Economy;
using Economy.Items;
using UnityEngine;

public class ResourceTransmitter : MonoBehaviour
{
    [SerializeField] private ItemType _typeFromPlayer;
    [SerializeField] private Converter _converter;

    [SerializeField, Header("Сколько пуха передается от игрока")]
    private int _fluffCount;  
    [SerializeField, Header("Время изготовления")]
    private int _delayProduction;

    private BuildStorage _storage;
    private Inventory _characterInventory;
    private ItemType _typeToPlayer;

    private int _fluff;
    private void Start() => _storage = GetComponent<BuildStorage>();

    private void CheckBag()
    {
        if (_characterInventory == null) return;

        _characterInventory.Add(_typeToPlayer, _storage.GetFluff());
        _storage.ResetFluff();
    }

    private void Make()
    {
        _typeToPlayer = ItemType.UncleanedFluff;

        if (_converter != null)
            _typeToPlayer = _converter.Convert(_typeFromPlayer);

        _characterInventory.GiveToStorage(ref _storage, _typeFromPlayer, _fluffCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _characterInventory = collision.gameObject.GetComponent<Inventory>();
        
        CheckBag();
        StartCoroutine(Production(_delayProduction));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_characterInventory == collision.GetComponent<Inventory>()) _characterInventory = null;
    }

    private IEnumerator Production(float delay)
    {
        yield return new WaitForSeconds(delay);
        Make();
    }
}