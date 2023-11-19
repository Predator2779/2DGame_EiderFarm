using Economy;
using Economy.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildStorage))]
public class Machine : MonoBehaviour
{
    [SerializeField] private BuildStorage _storage;
    [SerializeField] private Converter _converter;
    [SerializeField] private ResourceTransmitter _transmitter;

    private bool _isWorked;

    private void Start()
    {
        _storage = GetComponent<BuildStorage>();
        _converter = GetComponent<Converter>();
        _transmitter = GetComponent<ResourceTransmitter>();

        _transmitter.TransmitteEvent += Production;
    }

    [SerializeField, Header("Время изготовления")]
    private int _delayProduction;

    private ItemType _typeToPlayer;

    private void Make(ItemType _typeFromPlayer, int _fluffCount)
    {
        _typeToPlayer = _converter.Convert(_typeFromPlayer);
        _storage.AddFluff(_fluffCount);
    }

    private IEnumerator Production(ItemType _typeFromPlayer, Inventory _characterInventory, int _fluffCount)
    {
        Debug.Log(2);
        if (!_isWorked)
        {
            _isWorked = true;
            _characterInventory.RemoveItems(_typeFromPlayer, _fluffCount);
            yield return new WaitForSecondsRealtime(_delayProduction);
            Make(_typeFromPlayer, _fluffCount);
            _isWorked = false;
        }
    }
}
