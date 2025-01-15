using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerOrAIMenu : MonoBehaviour
{
    public void PlayOnMultiplayer()
    {
        SceneManager.LoadSceneAsync("Scenes/Menus/Play/Multiplayer/HostOrJoinMenu");
    }
    
    public void PlayWithAI()
    {
        SceneManager.LoadSceneAsync("Scenes/Menus/Play/Multiplayer/HostOrJoinMenu");
    }
}