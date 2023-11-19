using General;
using TMPro;
using UnityEngine;
using static General.GlobalConstants;

public class InventoryDrawer : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyCounter;
    [SerializeField] private TMP_Text _cleanFCounter;
    [SerializeField] private TMP_Text _uncleanFCounter;
    
    private void Awake()
    {
        EventHandler.OnInventoryAdd.AddListener(UpdateCounter);
    }

    private void UpdateCounter(string content, int points)
    {
        switch (content)
        {
            case Money:
                _moneyCounter.text = points.ToString();
                break;            
            case CleanedFluff:
                _cleanFCounter.text = points.ToString();
                break;            
            case UncleanedFluff:
                _uncleanFCounter.text = points.ToString();
                break;
        }
    }
}