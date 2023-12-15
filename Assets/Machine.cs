using Building.Constructions;
using Economy;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BuildStorage))]
[RequireComponent(typeof(Converter))]
[RequireComponent(typeof(ResourceTransmitter))]
public class Machine : MonoBehaviour
{
    [SerializeField, Header("����� ������������")] private int _delayProduction;
    
    private BuildStorage _storage;
    private Converter _converter;
    [SerializeField] private ResourceTransmitter _transmitter;
    private Item _typeFromPlayer;
    private bool _isWorked;

    [SerializeField] private Animator _animator;

    private Construction _construction;

    [Header("����� ����������� �� ���������� (������� ��������� �����)")]
    [SerializeField, Range(1, 100)] private int[] _upgradeTime;

    [Header("������� ���� �� ��� �� ���������� (������� ��������� �����)")]
    [SerializeField, Range(1, 100)] private int[] _ugradeFluffCount;

    private bool _isInitialized;

    private void Start() => Initialize();

    public void Initialize()
    {
        if (_isInitialized) return;
        
        _construction = GetComponent<Construction>();
        _storage = GetComponent<BuildStorage>();
        _converter = GetComponent<Converter>();
        _transmitter = GetComponent<ResourceTransmitter>();

        _transmitter.TransmitteEvent += Production;
        CheckGrade();
        
        _isInitialized = true;
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

    public void EnableAnimator()
    {
        _animator.enabled = !_animator.enabled;
        Debug.Log(1);
    }

    public void CheckGrade()
    {
        switch (_construction.GetCurrentGrade())
        {
            case 1: _delayProduction = _upgradeTime[0]; _transmitter.ChangeFluffCount(_ugradeFluffCount[0]); break;
            case 2: _delayProduction = _upgradeTime[1]; _transmitter.ChangeFluffCount(_ugradeFluffCount[1]); break;
            case 3: _delayProduction = _upgradeTime[2]; _transmitter.ChangeFluffCount(_ugradeFluffCount[2]); break;
        }
    }


}
