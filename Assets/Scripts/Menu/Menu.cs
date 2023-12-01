using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private static bool resetData;

    public void StartLevel(bool newGame)
    {
        resetData = newGame;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitLevel() => SceneManager.LoadScene(0);

    public void Exit() => Application.Quit();

    public bool GetResetValue() => resetData;
}