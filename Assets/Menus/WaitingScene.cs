using ForNetwork;
using Networking.Common.Client;
using Networking.Common.Server;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingScene : MonoBehaviour
{

    public Network Network = Network.Instance;
    
    //___________________________________________________________//
    //_________________________For Multi_________________________//
    //___________________________________________________________//
    
    void OnEnable(){
        Debug.Log("Starting by WaitingScene");
        Network.Setup = ConnectionAdded;
    }
    
    public void ConnectionAdded(Proxy proxy) {
        proxy.GameActionListenerManager.AddListener<ServerStartLobbyGameAction>((connection, action) => {
            InitGame();
        });
    }

    //___________________________________________________________//
    //___________________________________________________________//
    //___________________________________________________________//

    public void StartGame() // For Button
    {
        SceneManager.LoadScene("Scenes/BuildingsScene");
        /*
        if (Network.Instance.Proxy != null)
        {
            Network.Instance.Proxy.Connection.Send(new ClientWantsStartGameAction());
        }
        Debug.Log("envoi du gameAction");
        */
    }
    
    public void InitGame()
    {
        SceneManager.LoadScene("Scenes/BuildingsScene");
    }
}