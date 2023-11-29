using General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static General.GlobalConstants;

public class InventoryDrawer : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyCounter;
    [SerializeField] private TMP_Text _cleanFCounter;
    [SerializeField] private TMP_Text _uncleanFCounter;
    [SerializeField] private TMP_Text _clothesCounter;

    [SerializeField] private GameObject _flagPanel;
    [SerializeField] private Image _flagImage;
    
    private bool _isFlagAdded;

    private void Awake()
    {
        EventHandler.OnBunchChanged.AddListener(UpdateCounter);
        EventHandler.OnFlagChanged.AddListener(UpdateFlagSprite);
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
            case Clothes:
                _clothesCounter.text = points.ToString();
                break;
        }
    }

    private void UpdateFlagSprite(int count, Sprite[] flagSprites)
    {
        if(count > 0 && !_isFlagAdded)
        {
            _flagPanel.SetActive(true);
            _flagImage.sprite = flagSprites[Random.Range(0,flagSprites.Length)];
            _isFlagAdded = true;
        }
        else if(count <= 0)
        {
            _flagPanel.SetActive(false);
        }
    }
    public Sprite GetSprite()
    {
        _isFlagAdded = false;
        return _flagImage.sprite;
    }

    public GameObject GetFlagPanel() => _flagPanel;
}