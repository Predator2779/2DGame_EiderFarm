using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
    [SerializeField] Sprite[] _arrows;
    [SerializeField] Image _image;

    private bool isShop;
    
    public void ChangeSprite()
    {
        if (!isShop)
        {
            isShop = true;
            _image.sprite = _arrows[0];
        }
        else
        {
            isShop = false;
            _image.sprite = _arrows[1];
        }
    }
}
