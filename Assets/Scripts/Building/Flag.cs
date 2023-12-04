using Building;
using Economy;
using General;
using UnityEngine;
using UnityEngine.Serialization;
using EventHandler = General.EventHandler;

public class Flag : MonoBehaviour
{
    [SerializeField] private BuildMenu _buildMenu;
    [SerializeField] private GameObject _flagBtn;
    [SerializeField] private GameObject _flag;

    private Inventory _playerInventory;
    private ItemBunch _itemBunch;
    private Sprite _sprite;

    public bool isFlagAdded;

    private void Awake()
    {
        if (!isFlagAdded)
            EventHandler.OnFlagSpriteChanged.AddListener(SetFlagSprite);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<InputHandler>()) return;
        _playerInventory = other.gameObject.GetComponent<Inventory>();

        if (!_playerInventory.TryGetBunch(GlobalConstants.Flag, out var bunch) ||
            bunch.GetCount() <= 0) return;

        _itemBunch = bunch;

        if (!_buildMenu.IsBuilded || isFlagAdded) return;

        _flagBtn.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other) => _flagBtn.SetActive(false);

    public void SetFlag()
    {
        if (_itemBunch == null || _itemBunch.GetCount() <= 0) return;

        isFlagAdded = true;
        _itemBunch.RemoveItems(1);
        _flagBtn.SetActive(false);
        _flag.SetActive(true);
        EventHandler.FlagPanelEvent.Invoke(false);
        EventHandler.OnFlagSet?.Invoke();
        EventHandler.OnFlagSpriteChanged.RemoveListener(SetFlagSprite);
    }

    public void RemoveFlag()
    {
        _flag.SetActive(false);
        isFlagAdded = false;

    }
    public void AddFlag()
    {
        SetFlag();
        
        // _flag.SetActive(true);
        // isFlagAdded = true;
        // EventHandler.OnFlagSpriteChanged?.RemoveListener(SetFlagSprite);
        // EventHandler.OnFlagSet?.Invoke();
    }

    public Sprite GetSprite()
    {
        return gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public void SetSprite(Sprite spr)
    {
        gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = spr;
    }

    public GameObject GetFlagButton() => _flagBtn;

    private void SetFlagSprite(Sprite sprite) => _flag.GetComponent<SpriteRenderer>().sprite = sprite;
}