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
            CSteamID id = SteamUser.GetSteamID();
            SteamFriends.ActivateGameOverlayInviteDialog(id);
            
            //SteamFriends.ActivateGameOverlay("Friends");
            
            
            
            SceneManager.LoadScene("WaitingScene");
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