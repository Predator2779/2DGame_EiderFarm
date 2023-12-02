using Building;
using Economy;
using General;
using UnityEngine;
using EventHandler = General.EventHandler;

public class Flag : MonoBehaviour
{
    [SerializeField] private BuildMenu _buildMenu;
    [SerializeField] private GameObject _flagBtn;
    [SerializeField] private GameObject _flag;

    private Inventory _playerInventory;
    private ItemBunch _itemBunch;
    private bool _isFlagAdded;

    private void Awake() => EventHandler.OnFlagSpriteChanged.AddListener(SetFlagSprite);

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<InputHandler>() ||
            !_buildMenu.IsBuilded ||
            _isFlagAdded) return;

        _playerInventory = other.gameObject.GetComponent<Inventory>();
        
        if (!_playerInventory.TryGetBunch(GlobalConstants.Flag, out var bunch) ||
            bunch.GetCount() <= 0) return;
        
        _flagBtn.SetActive(true);
        _itemBunch = bunch;
    }

    private void OnTriggerExit2D(Collider2D other) => _flagBtn.SetActive(false);

    public void SetFlag()
    {
        if (_itemBunch == null || _itemBunch.GetCount() <= 0) return;

        _flagBtn.SetActive(false);
        EventHandler.FlagPanelEvent.Invoke(false);
        _isFlagAdded = true;

        _flag.SetActive(true);
        _itemBunch.RemoveItems(1);
        EventHandler.OnFlagSet?.Invoke();
    }

    private void SetFlagSprite(Sprite sprite) => _flag.GetComponent<SpriteRenderer>().sprite = sprite;
}