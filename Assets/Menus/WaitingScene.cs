using ForNetwork;
using ForServer;
using Grid;
using Networking.Common.Client;
using Networking.Common.Server;
using OlympusWorldGenerator;
using OlympusWorldGenerator.Generators;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingScene : MonoBehaviour
{
    //___________________________________________________________//
    //_________________________For Multi_________________________//
    //___________________________________________________________//
    
    void OnEnable()
    {

        if (Network.Instance.Proxy is not null)
        {
            Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerStartLobbyGameAction>(
                (connection, action) =>
                {
                    Debug.Log("[CLIENT]     : Received ServerStartLobbyGameAction");
                    ServerManager.Seed = (int)action.Seed;
                    ServerManager.MapWidth = 10;
                    ServerManager.MapHeight = 10;
                    ServerManager.PlayerCount = action.PlayerCount;
                    ServerManager.PlayerId = action.PlayerId;
                    InitGame();
                });
        }
        else
        {
            Network.Instance.Setup = (proxy =>
            {
                proxy.GameActionListenerManager.AddListener<ServerStartLobbyGameAction>(
                    (connection, action) =>
                    {
                        Debug.Log("[CLIENT]     : Received ServerStartLobbyGameAction");
                        ServerManager.Seed = (int)action.Seed;
                        ServerManager.MapWidth = 10;
                        ServerManager.MapHeight = 10;
                        ServerManager.PlayerCount = action.PlayerCount;
                        ServerManager.PlayerId = action.PlayerId;
                        InitGame();
                    });
            });
        }
        
    }
    
    //___________________________________________________________//
    //___________________________________________________________//
    //___________________________________________________________//

    public void Ready() // For Button
    {
        Network.Instance.Proxy.Connection.Send(new ClientReadyStateGameAction(true));
        Debug.Log("Envoi du gameAction pour Ready");
    }
    
    public void NotReady() // For Button
    {
        Network.Instance.Proxy.Connection.Send(new ClientReadyStateGameAction(false));
        Debug.Log("Envoi du gameAction pour Not Ready");
    }
    
    public void InitGame()
    {
        SceneManager.LoadScene("PlayGame");
    }
}