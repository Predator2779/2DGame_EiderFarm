using Building.Constructions;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BuildStorage))]
[RequireComponent(typeof(ResourceTransmitter))]
public class FluffGiver : MonoBehaviour
{
    [Header("Шанс выпадения пуха (в процентах).")]
    [SerializeField] private int _chance;

    [Header("Время выпадения пуха.")]
    private int _time;

    [Header("Количество воспроизводимого пуха.")]
    private int _fluffCount;

    [Header("Время выпадения пуха по улучшениям (открыть стрелочку слева)")]
    [SerializeField, Range(1,100)] private int[] _upgradeTime;

    [Header("Количество воспроизв. пуха по улучшениям (открыть стрелочку слева)")]
    [SerializeField, Range(1,100)] private int[] _ugradeFluffCount;

    private BuildStorage _storage;
    private ResourceTransmitter _transmitter;
    private bool hasGivenFluff;

    [SerializeField] private Sprite[] _spritesWithFluff;
    private SpriteRenderer _sprRender;
    private Construction _construction;

    private void Start()
    {
        _storage = GetComponent<BuildStorage>();
        _transmitter = GetComponent<ResourceTransmitter>();
        _sprRender = GetComponent<SpriteRenderer>();
        _construction = GetComponent<Construction>();
        CheckGrade();
        StartCoroutine(CreateFluff());
    }
    
    private void GiveFluff()
    {
        if (hasGivenFluff) return;

        

        hasGivenFluff = true;
        if (Random.Range(0, 100) < _chance)
        {
            _storage.AddFluff(_fluffCount);
            _transmitter.CheckBag();
            ChangeSpriteFromNullToFluff();
        }
        hasGivenFluff = false;
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
