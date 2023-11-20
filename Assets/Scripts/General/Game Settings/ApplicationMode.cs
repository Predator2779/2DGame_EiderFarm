using UnityEngine;
using UnityEngine.UI;

public class ApplicationMode : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private AudioSource _audioSource;

    public void SwitchMode() => _menuPanel.SetActive(!_menuPanel.activeSelf);

    public void ChangeVolume(Slider slider) => _audioSource.volume = slider.value;
    
    public void Quit() => Application.Quit();
}