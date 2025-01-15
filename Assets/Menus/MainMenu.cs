using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Scenes/Menus/Play/MultiplayerOrAIMenu");
    }
    public void OpenOptions()
    {
        SceneManager.LoadSceneAsync("Scenes/Menus/OptionMenu");
    }
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu !");
        Application.Quit();
    }
}
