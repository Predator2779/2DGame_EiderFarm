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
    [SerializeField] private float _time;

    [Header("Количество воспроизводимого пуха.")]
    [SerializeField] private int _fluffCount;

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
            StartCoroutine(ChangeSpritesWithDelay(0.2f));
        }
        hasGivenFluff = false;
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
        ChangeSpriteFromNullToFluff();
    }
}
