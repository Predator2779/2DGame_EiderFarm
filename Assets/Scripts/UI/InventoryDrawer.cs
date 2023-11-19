using Economy.Items;
using General;
using TMPro;
using UnityEngine;

public class InventoryDrawer : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyCounter;
    [SerializeField] private TMP_Text _cleanFCounter;
    [SerializeField] private TMP_Text _uncleanFCounter;
    
    private void Awake()
    {
        EventHandler.OnInventoryAdd.AddListener(UpdateCounter);
    }

    private void UpdateCounter(ItemType content, int points)
    {
        switch (content)
        {
            case ItemType.Money:
                _moneyCounter.text = points.ToString();
                break;            
            case ItemType.CleanedFluff:
                _cleanFCounter.text = points.ToString();
                break;            
            case ItemType.UncleanedFluff:
                _uncleanFCounter.text = points.ToString();
                break;
        }
    }
}