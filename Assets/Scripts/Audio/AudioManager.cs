using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] GameObject _audio;

    [SerializeField] Sprite[] _sprites;

    [SerializeField] Image _buttonImage;

    public void SwitchAudioMod()
    {
        if (_buttonImage.sprite == _sprites[0])
        {
            _buttonImage.sprite = _sprites[1];
            _audio.SetActive(false);
        }
        else
        {
            _buttonImage.sprite = _sprites[0];
            _audio.SetActive(true);
        }
    }
}
