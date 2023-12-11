using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Economy.Farm_House
{
    public class MenuCell : MonoBehaviour
    {
        [SerializeField] protected Image _icon;
        [SerializeField] protected TMP_Text _name;
        [SerializeField] protected TMP_Text _description;
        [SerializeField] protected TMP_Text _counter;

        [SerializeField] protected TMP_Text _price;
        
        protected void SetButton(string description)
        {
            _description.text = description;
        }    
        
        protected void SetButton(Sprite icon, string description)
        {
            if(_icon != null)
            _icon.sprite = icon;
            if(_description != null)
            _description.text = description;
        }
        
        protected void SetButton(Sprite icon, string name, string description)
        {
            if(_name != null)
            _name.text = name;
            SetButton(icon, description);
        }   
        
        protected void SetButton(Sprite icon, string description, int count)
        {
            SetButton(icon, description);
            _counter.text = count.ToString();
        }     
        
        protected void SetButton(Sprite icon, string name, string description, int count)
        {
            if (_name != null)
                _name.text = name;
            SetButton(icon, description, count);
        }

        protected void SetButton(Sprite icon, string name, string description, int count, int price, bool isShop)
        {
            if (_name != null)
                _name.text = name;

            if(_price != null)
                _price.text = price.ToString();
            SetButton(icon, description, count);
        }

    }
}