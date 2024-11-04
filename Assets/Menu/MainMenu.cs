using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Scenes/SampleScene");
    }
    public void OpenOptions()
    {
        SceneManager.LoadSceneAsync("Scenes/MenuOption");
    }
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu !");
        Application.Quit();
    }
}
