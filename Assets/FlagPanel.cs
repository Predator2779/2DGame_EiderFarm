using Economy;
using General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPanel : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    private void Awake()
    {
        EventHandler.OnFlagPanelCurrentState.AddListener(CheckFlagPanelState);
    }
    public void CheckFlagPanelState()
    {
        if(_inventory.TryGetBunch(GlobalConstants.Flag, out var bunch))
        {
            if (bunch.GetCount() < 1)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }
        else
            gameObject.SetActive(false);
    }
}
