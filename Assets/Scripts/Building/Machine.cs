using Building.Constructions;
using Economy;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

[RequireComponent(typeof(BuildStorage))]
[RequireComponent(typeof(Converter))]
[RequireComponent(typeof(ResourceTransmitter))]
public class Machine : MonoBehaviour
{
    [Header("Время переработки по улучшениям (открыть стрелочку слева)")]
    [SerializeField, Range(1, 100)] private int[] _upgradeTime;

    [Header("Сколько пуха за раз по улучшениям (открыть стрелочку слева)")]
    [SerializeField, Range(1, 100)] private int[] _ugradeFluffCount;

    [SerializeField, Header("Время изготовления")]
    private int _delayProduction;

    [SerializeField] private string _soundProcessing;
    [SerializeField] private Animator _animator;

    private Construction _construction;
    private BuildStorage _storage;
    private Converter _converter;
    private ResourceTransmitter _transmitter;
    private Item _typeFromPlayer;
    private EventInstance _eventInstance;
    private bool _isWorked;
    private bool _isInitialized;

    private void Start() => Initialize();

    public void Initialize()
    {
        if (_isInitialized) return;

        _construction = GetComponent<Construction>();
        _storage = GetComponent<BuildStorage>();
        _converter = GetComponent<Converter>();
        _transmitter = GetComponent<ResourceTransmitter>();

        _eventInstance = RuntimeManager.CreateInstance(_soundProcessing);
        RuntimeManager.AttachInstanceToGameObject(_eventInstance, transform);

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

    public void StartSound() => _eventInstance.start();
    public void StopSound() => _eventInstance.stop(STOP_MODE.IMMEDIATE);

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

    public void CheckGrade()
    {
        switch (_construction.GetCurrentGrade())
        {
            case 1:
                _delayProduction = _upgradeTime[0];
                _transmitter.ChangeFluffCount(_ugradeFluffCount[0]);
                break;
            case 2:
                _delayProduction = _upgradeTime[1];
                _transmitter.ChangeFluffCount(_ugradeFluffCount[1]);
                break;
            case 3:
                _delayProduction = _upgradeTime[2];
                _transmitter.ChangeFluffCount(_ugradeFluffCount[2]);
                break;
        }
    }
}