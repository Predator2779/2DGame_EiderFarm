using System.Collections;
using Building.Constructions;
using Characters;
using Economy;
using General;
using Other;
using UnityEngine;

[RequireComponent(typeof(Construction))]
[RequireComponent(typeof(BuildStorage))]
public class ResourceTransmitter : MonoBehaviour
{
    public delegate IEnumerator CoroutineDelegate(Item typeFrom, Inventory inv, int fluff);
    public event CoroutineDelegate TransmitteEvent;

    [SerializeField, Header("������� ���� ���������� �� ������")]
    private int _fluffCount;

    [SerializeField] private Item _typeToPlayer;
    [SerializeField] private Inventory _characterInventory;
    [SerializeField] private Person _person;
    
    private Construction _construction;
    private BuildStorage _storage;
    private Machine _machine;
    private Sprite _sprite;
    private BubbleWrap _bubbles;

    public void Awake()
    {
        _construction = GetComponent<Construction>();
        _storage = GetComponent<BuildStorage>();
        _machine = GetComponent<Machine>();
        _bubbles = GetComponent<BubbleWrap>();
    }

    public bool CheckBag()
    {
        if (_characterInventory == null && _person == null) return false;

        if (_construction.typeConstruction == GlobalTypes.TypeBuildings.Storage 
            && _person.TryGetComponent(out Employee employee))
        {
            TransmitteFromEmployee(employee);
            return false; /// �� ������� �������
        }

        Transmitte();

        if (GetComponent<Converter>())
            if (_characterInventory.GetBunch(GetComponent<Converter>().GetRelevantItem()).GetCount() < _fluffCount)
            {
                _machine.GetAnimator().enabled = false;
                return false;
            }

        if (_fluffCount != 0 && TransmitteEvent != null)
            StartCoroutine(TransmitteEvent?.Invoke(_typeToPlayer, _characterInventory, _fluffCount));

        return true;
    }

    private void TransmitteFromEmployee(Employee employee)
    {
        var inventory = employee.GetComponent<Inventory>();
        if (!inventory.TryGetBunch(GlobalConstants.CleanedFluff, out ItemBunch bunch) || bunch == null) return;
        
        _storage.AddFluff(bunch.GetCount());
        inventory.RemoveItems(bunch.GetItem(), bunch.GetCount());
        
        // inventory.Exchange(inventory, gameObject.GetComponent<Inventory>(), bunch);
    }

    private void Transmitte()
    {
        if (_storage.GetFluffCount() == 0) return;

        int count = _storage.GetFluffCount();

        _characterInventory.AddItems(_typeToPlayer, count);
        _storage.ResetFluff();

        if (_bubbles != null) _bubbles.StartBubble();
        
        if (TryGetComponent(out FluffGiver fluffGiver)) StartCoroutine(fluffGiver.ChangeSpritesWithDelay(0.3f));

        EventHandler.OnItemTransmitted?.Invoke(_construction.typeConstruction, _typeToPlayer, count);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out _person)) return;

        _characterInventory = collision.gameObject.GetComponent<Inventory>();

        if (!CheckBag()) return;

        if (gameObject.GetComponent<Machine>()) _machine.GetAnimator().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_characterInventory != collision.GetComponent<Inventory>() ||
            collision.gameObject.GetComponent<Person>() != _person) return;

        _characterInventory = null;
        _person = null;

        if (gameObject.GetComponent<Machine>()) _machine.GetAnimator().enabled = false;
    }

    public int GetRequireFluffCount() => _fluffCount;
    public void SetGradeAnimationTrue(int grade) => _machine.Animation(true, grade);
    public void ChangeFluffCount(int count) => _fluffCount = count;
}