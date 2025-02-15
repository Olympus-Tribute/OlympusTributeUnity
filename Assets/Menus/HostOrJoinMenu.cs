using ForNetwork;
using ForServer;
using Networking.API;
using Networking.API.Listeners;
using Networking.Local;
using OlympusDedicatedServer;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Proxy = ForNetwork.Proxy;

public class HostOrJoinMenu : MonoBehaviour
{
    private Camera _mainCamera;
    

    private void Start()
    {
		_mainCamera = Camera.main;
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
            SteamFriends.ActivateGameOverlay("Friends");
            SceneManager.LoadScene("WaitingScene");
        }
        else
        {
            Debug.LogError("Steam is not initialized or not running.");
        }
    }
    
    public void Host()
    {
        CSteamID id = SteamUser.GetSteamID();
        //SteamFriends.ActivateGameOverlayInviteDialog(id);
        
        ServerManager.Instance.Host();
        
        SceneManager.LoadScene("WaitingScene");
    }
    
    public void Join()
    {
        OpenFriendsOverlay();
    }
}