using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private string _loadedScene;
    [SerializeField] private string _cutScene;
    
    private static bool _isNewGame;

    public void StartLevel(bool isNewGame)
    {
        _isNewGame = isNewGame;

        SceneManager.LoadScene(isNewGame ? _cutScene : _loadedScene);
    }

    public void ExitLevel() => SceneManager.LoadScene(1);

    public void Exit() => Application.Quit();

    public bool IsNewGame() => _isNewGame;
}