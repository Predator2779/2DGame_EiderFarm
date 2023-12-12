using UnityEngine;

public class ApplicationMode : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;

    public void SwitchMode() => _menuPanel.SetActive(!_menuPanel.activeSelf);
    
    public void Quit() => Application.Quit();
}