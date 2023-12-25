using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string _loadedScene;
    [SerializeField] private string _menuScene;
    [SerializeField] private string _cutScene;
    
    [Space][Header("Settings")]
    [SerializeField] private Button _loadGameButton;
    [SerializeField] private TextMeshProUGUI _textButton;
    [SerializeField] private SaveSerial _saveSerial;
    
    private static bool _isNewGame;
    private static bool _isHasSaves;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            _saveSerial.Initialize();
    }
    public void IfNotSaves()
    {
        if (_loadGameButton == null || _textButton == null) return;
        
        _loadGameButton.interactable = false;
        _textButton.color = Color.gray;
    }

    public void StartLevel(bool isNewGame)
    {
        _isNewGame = isNewGame;
        SceneManager.LoadScene(isNewGame ? _cutScene : _loadedScene);
    }

    public void ExitLevel() => SceneManager.LoadScene(_menuScene);
    public void Exit() => Application.Quit();
    public bool IsNewGame() => _isNewGame;
    public bool IsHasSaves() => _isHasSaves;
    public void SetSaves(bool isSaves) => _isHasSaves = isSaves;
}