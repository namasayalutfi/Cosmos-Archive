using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Fungsi untuk tombol Mulai
    public void PlayGame()
    {
        // Load scene index 1 (Solar System)
        SceneManager.LoadScene(1);
    }

    // Fungsi untuk tombol Keluar
    public void QuitGame()
    {
        Debug.Log("Keluar program..."); // Agar terlihat di editor
        Application.Quit();
    }
}