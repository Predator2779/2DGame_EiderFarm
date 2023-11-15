using Economy;
using General;
using TMPro;
using UnityEngine;

public class BagDrawer : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyCounter;
    [SerializeField] private TMP_Text _cleanFCounter;
    [SerializeField] private TMP_Text _uncleanFCounter;
    [SerializeField] private TMP_Text _clothesCouunter;
    
    private void Start()
    {
        EventHandler.OnBagAdd.AddListener(UpdateCounter);
    }

    private void UpdateCounter(BagContent content, int points)
    {
        switch (content)
        {
            case BagContent.Money:
                _moneyCounter.text = points.ToString();
                break;            
            case BagContent.CleanedFluff:
                _cleanFCounter.text = points.ToString();
                break;            
            case BagContent.UncleanedFluff:
                _uncleanFCounter.text = points.ToString();
                break;            
            case BagContent.Clothes:
                _clothesCouunter.text = points.ToString();
                break;
        }
    }
}