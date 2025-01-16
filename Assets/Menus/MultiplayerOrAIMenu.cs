using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerOrAIMenu : MonoBehaviour
{
    public void PlayOnMultiplayer()
    {
        SceneManager.LoadScene("Scenes/Menus/Play/Multiplayer/HostOrJoinMenu");
    }
    
    public void PlayWithAI()
    {
        SceneManager.LoadScene("Scenes/Menus/Play/Multiplayer/HostOrJoinMenu");
    }
    
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Scenes/Menus/MainMenu");
    }
}