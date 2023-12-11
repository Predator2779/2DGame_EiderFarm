using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private static bool _isNewGame;

    public void StartLevel(bool isNewGame)
    {
        _isNewGame = isNewGame;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitLevel() => SceneManager.LoadScene(1);

    public void Exit() => Application.Quit();

    public bool IsNewGame() => _isNewGame;
}