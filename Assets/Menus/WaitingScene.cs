using ForNetwork;
using ForServer;
using Networking.Common.Client;
using Networking.Common.Server;
using PopUp;
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
                    Debug.Log("Received ServerStartLobbyGameAction");
                    ServerManager.Seed = action.Seed;
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
                        Debug.Log("Received ServerStartLobbyGameAction");
                        ServerManager.Seed = action.Seed;
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
        if (Network.Instance.Proxy is not null)
        {
            Network.Instance.Proxy.Connection.Send(new ClientReadyStateGameAction(true));
            Debug.Log("Envoi du gameAction pour Ready");
        }
        else
        {
            PopUpManager.Instance.ShowPopUp("Not connected to Server");
        }
        
    }
    
    public void NotReady() // For Button
    {
        if (Network.Instance.Proxy is not null)
        {
            Network.Instance.Proxy.Connection.Send(new ClientReadyStateGameAction(false));
            Debug.Log("Envoi du gameAction pour Ready");
        }
        else
        {
            PopUpManager.Instance.ShowPopUp("Not connected to Server");
        }
    }
    
    public void InitGame()
    {
        Debug.Log(ServerManager.Seed);
        Debug.Log(ServerManager.PlayerCount);
        Debug.Log(ServerManager.PlayerId);
        
        // génere ici

        SceneManager.LoadScene("Scenes/BuildingsScene");
    }
}