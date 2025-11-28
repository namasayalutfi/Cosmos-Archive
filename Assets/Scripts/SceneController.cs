using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Solar System");
    }

    public void QuitGame()
    {
        Debug.Log("Keluar program...");
        Application.Quit();
    }
}