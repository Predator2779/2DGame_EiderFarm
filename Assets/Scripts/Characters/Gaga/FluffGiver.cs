using Building.Constructions;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BuildStorage))]
[RequireComponent(typeof(ResourceTransmitter))]
public class FluffGiver : MonoBehaviour
{
    [Header("���� ��������� ���� (� ���������).")]
    [SerializeField] private int _chance;

    [Header("����� ��������� ����.")]
    private int _time;

    [Header("���������� ���������������� ����.")]
    private int _fluffCount;

    [Header("����� ��������� ���� �� ���������� (������� ��������� �����)")]
    [SerializeField, Range(1,100)] private int[] _upgradeTime;

    [Header("���������� ���������. ���� �� ���������� (������� ��������� �����)")]
    [SerializeField, Range(1,100)] private int[] _ugradeFluffCount;

    [SerializeField] private Sprite[] _spritesWithFluff;
    
    private BuildStorage _storage;
    private ResourceTransmitter _transmitter;
    private SpriteRenderer _sprRender;
    private Construction _construction;
    private bool _hasGivenFluff;
    private bool _isInitialized;

    private void Start() => Initialize();

    public void Initialize()
    {
        if (_isInitialized) return;
        
        _storage = GetComponent<BuildStorage>();
        _transmitter = GetComponent<ResourceTransmitter>();
        _sprRender = GetComponent<SpriteRenderer>();
        _construction = GetComponent<Construction>();
        
        CheckGrade();
        StartCoroutine(CreateFluff());
        
        _isInitialized = true;
    }
    
    private void GiveFluff()
    {
        if (_hasGivenFluff) return;

        _hasGivenFluff = true;
        if (Random.Range(0, 100) < _chance)
        {
            _storage.AddFluff(_fluffCount);
            _transmitter.CheckBag();
            ChangeSpriteFromNullToFluff();
        }
        _hasGivenFluff = false;
    }

    public void CheckGrade()
    {
        switch (_construction.GetCurrentGrade())
        {
            case 1: _time = _upgradeTime[0]; _fluffCount = _ugradeFluffCount[0]; break;
            case 2: _time = _upgradeTime[1]; _fluffCount = _ugradeFluffCount[1]; break;
            case 3: _time = _upgradeTime[2]; _fluffCount = _ugradeFluffCount[2]; break;
        }
    }

    private IEnumerator CreateFluff()
    {
        yield return new WaitForSecondsRealtime(_time);
        GiveFluff();
        StartCoroutine(CreateFluff());
    }

    public void ChangeChance(int chance)
    {
        _chance = chance;
    }

    public void ChangeSpriteFromFluffToNull()
    {
        _sprRender.sprite = _construction.GetCurrentGradeSprite(_construction.GetGradeBuildings());
    }
    public void ChangeSpriteFromNullToFluff()
    {
        _sprRender.sprite = _construction.GetCurrentGradeSprite(_spritesWithFluff);
    }

    public IEnumerator ChangeSpritesWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        ChangeSpriteFromFluffToNull();
    }
}
