using Economy;
using General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPanel : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    private void Awake() => EventHandler.OnFlagPanelCurrentState.AddListener(CheckFlagPanelState);

    private void CheckFlagPanelState()
    {
        if (_inventory.TryGetBunch(GlobalConstants.Flag, out var bunch))
        {
            gameObject.SetActive(bunch.GetCount() >= 1);
            return;
        }

        gameObject.SetActive(false);
    }
}