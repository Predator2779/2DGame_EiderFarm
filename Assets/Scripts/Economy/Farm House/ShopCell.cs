using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Economy.Farm_House
{
    public class ShopCell : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _counter;
        [SerializeField] private TMP_InputField _inpField;

        public void SetCell(Sprite icon, string description, int count)
        {
            _icon.sprite = icon;
            _description.text = description;
            _counter.text = count.ToString();
        }
    }
}