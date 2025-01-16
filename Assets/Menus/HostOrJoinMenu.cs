using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostOrJoinMenu : MonoBehaviour
{
    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam is not initialized.");
            return;
        }
    }

    public void OpenFriendsOverlay()
    {
        if (SteamManager.Initialized)
        {
            //SteamFriends.ActivateGameOverlay("Friends");
            SceneManager.LoadScene("Scenes/BuildingsScene");
        }
        else
        {
            Debug.LogError("Steam is not initialized or not running.");
        }
    }
    
    public void Host()
    {
        OpenFriendsOverlay();
    }
    
    public void Join()
    {
        OpenFriendsOverlay();
    }
}