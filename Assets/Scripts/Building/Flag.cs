using Building;
using Economy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;
using UnityEngine.UI;
using EventHandler = General.EventHandler;

public class Flag : MonoBehaviour
{
    [SerializeField] private GameObject _setFlagButton;
    [SerializeField] private GameObject _flag;
    [SerializeField] private BuildMenu _buildMenu;
    [SerializeField] private InventoryDrawer _inventoryDrawer;

    private Inventory _playerInventory;

    private bool isFlagAdded;

    protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<InputHandler>() && other.gameObject.GetComponent<Inventory>().GetAllItems()[4].GetCount() > 0 && _buildMenu.IsBuilded && !isFlagAdded)
        {
            _playerInventory = other.gameObject.GetComponent<Inventory>();
            _setFlagButton.SetActive(true);
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
        _inventoryDrawer.GetFlagPanel().SetActive(false);
        isFlagAdded = true;
        _flag.GetComponent<SpriteRenderer>().sprite = _inventoryDrawer.GetSprite();
        _flag.SetActive(true);
        _playerInventory.GetAllItems()[4].RemoveItems(1);
        EventHandler.OnFlagSet?.Invoke();
    }


}
