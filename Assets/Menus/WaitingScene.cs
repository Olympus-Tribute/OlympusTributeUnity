using ForNetwork;
using ForServer;
using Networking.Common.Client;
using Networking.Common.Server;
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
        // cas spéciel car ,n'existe pas si join
        Network.Instance.Setup = (proxy =>
        {
            // = Network.Instance.Proxy.GameActionListenerManager.AddListener<ServerStartLobbyGameAction>(
            proxy.GameActionListenerManager.AddListener<ServerStartLobbyGameAction>(
                (connection, action) =>
                {
                    ServerManager.Seed = action.Seed;
                    ServerManager.PlayerCount = action.PlayerCount;
                    ServerManager.PlayerId = action.PlayerId;
                    InitGame();
                });
        });
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
        Debug.Log(ServerManager.Seed);
        Debug.Log(ServerManager.PlayerCount);
        Debug.Log(ServerManager.PlayerId);
        
        // génere ici

        SceneManager.LoadScene("Scenes/BuildingsScene");
    }
}