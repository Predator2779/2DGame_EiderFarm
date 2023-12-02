using Building;
using Economy;
using UnityEngine;
using EventHandler = General.EventHandler;

public class Flag : MonoBehaviour
{
    [SerializeField] private GameObject _setFlagButton;
    [SerializeField] private GameObject _flag;
    [SerializeField] private BuildMenu _buildMenu;
    [SerializeField] private InventoryDrawer _inventoryDrawer; //??

    private Inventory _playerInventory;

    private bool isFlagAdded;

    private ItemBunch _itemBunch;

    private void Awake()
    {
        EventHandler.OnFlagSpriteChanged.AddListener(SetFlagSprite);
    }
    protected void OnTriggerStay2D(Collider2D other)
    {

        if (other.GetComponent<InputHandler>() && _buildMenu.IsBuilded && !isFlagAdded)
        {
            _playerInventory = other.gameObject.GetComponent<Inventory>();
            if (_playerInventory.TryGetBunch("Ôëàæîê", out ItemBunch bunch))
            {
                _itemBunch = bunch;
                if (bunch.GetCount() > 0)
                    _setFlagButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<InputHandler>())
        {
            _setFlagButton.SetActive(false);
        }
    }

    public void SetFlag()
    {
        _setFlagButton.SetActive(false);
        EventHandler.FlagPanelEvent.Invoke(false);
        isFlagAdded = true;
        
        _flag.SetActive(true);
        _itemBunch.RemoveItems(1);
        EventHandler.OnFlagSet?.Invoke();
    }


    public void SetFlagSprite(Sprite sprite) => _flag.GetComponent<SpriteRenderer>().sprite = sprite;


}
