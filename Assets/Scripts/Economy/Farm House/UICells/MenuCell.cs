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
        
        protected void SetButton(Sprite icon, string description)
        {
            _icon.sprite = icon;
            _description.text = description;
        }
        
        protected void SetButton(Sprite icon, string name, string description)
        {
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
            _name.text = name;
            SetButton(icon, description, count);
        }
    }
}