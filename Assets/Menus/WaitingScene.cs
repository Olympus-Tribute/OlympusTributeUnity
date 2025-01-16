using ForNetwork;
using Networking.Common.Client;
using Networking.Common.Server;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingScene : MonoBehaviour
{

    public Network MenuNetwork = Network.Instance;
    
    //___________________________________________________________//
    //_________________________For Multi_________________________//
    //___________________________________________________________//
    
    void OnEnable(){
        Debug.Log("Starting by WaitingScene");
        MenuNetwork.Start(ConnectionAdded);
    }

    void Update(){
        
        MenuNetwork.Update();
    }
    
    public void ConnectionAdded(Proxy proxy) {
        proxy.GameActionListenerManager.AddListener<ServerPlaceBuildingGameAction>((connection, action) => {
            StartGame();
        });
    }

    //___________________________________________________________//
    //___________________________________________________________//
    //___________________________________________________________//

    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/BuildingsScene");
    }

    public void PlayGame()
    {
        // ajouter bouton dans unity
        Network.Instance.Connection.Connection.Send(new ClientPlaceBuildingGameAction());
    }
}