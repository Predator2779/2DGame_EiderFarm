using Economy;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BuildStorage))]
[RequireComponent(typeof(Converter))]
[RequireComponent(typeof(ResourceTransmitter))]
public class Machine : MonoBehaviour
{
    [SerializeField, Header("Время изготовления")] private int _delayProduction;
    
    private BuildStorage _storage;
    private Converter _converter;
    [SerializeField] private ResourceTransmitter _transmitter;
    private Item _typeFromPlayer;
    private bool _isWorked;

    [SerializeField] private Animator _animator;


    private void Start()
    {
        _storage = GetComponent<BuildStorage>();
        _converter = GetComponent<Converter>();
        _transmitter = GetComponent<ResourceTransmitter>();

        _transmitter.TransmitteEvent += Production;
    }

    private void Make(int _fluffCount)
    {
        _storage.AddFluff(_fluffCount);
        _transmitter.CheckBag();
    }

    public void Animation(bool work, int currentGrade)
    {
        _animator.SetBool("isWork", work);
        _animator.SetInteger("currentGrade", currentGrade);
    }

    private IEnumerator Production(Item _typeToPlayer, Inventory _characterInventory, int _fluffCount)
    {
        if (!_isWorked)
        {
            _isWorked = true;
            _typeFromPlayer = _converter.Convert(_typeToPlayer, _storage);
            _characterInventory.RemoveItems(_typeFromPlayer, _fluffCount);
            yield return new WaitForSecondsRealtime(_delayProduction);
            _isWorked = false;
            Make(_fluffCount);
        }
    }

    private void OnDestroy() => _transmitter.TransmitteEvent -= Production;

    public Animator GetAnimator() => _animator; 
}
