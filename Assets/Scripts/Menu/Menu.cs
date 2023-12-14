using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private string _loadedScene;
    [SerializeField] private string _cutScene;

    [SerializeField] private Button _loadGameButton;

    

    private static bool _isNewGame;

    private static bool _isHasSaves;


    private void Awake()
    {
        if (_loadGameButton != null)
            if (_isHasSaves)
                _loadGameButton.interactable = true;
    }

    public void StartLevel(bool isNewGame)
    {
        _isNewGame = isNewGame;

        SceneManager.LoadScene(isNewGame ? _cutScene : _loadedScene);
    }

    public void ExitLevel() => SceneManager.LoadScene(1);

    public void Exit() => Application.Quit();

    public bool IsNewGame() => _isNewGame;

    public bool IsHasSaves() => _isHasSaves;

    public void SetSaves(bool isSaves) => _isHasSaves = isSaves;
}