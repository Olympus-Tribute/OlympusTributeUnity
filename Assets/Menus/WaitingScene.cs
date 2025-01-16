using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingScene : MonoBehaviour
{

    public MenuNetwork MenuNetwork = new MenuNetwork();
    
    //___________________________________________________________//
    //_________________________For Multi_________________________//
    //___________________________________________________________//
    
    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam is not initialized.");
            return;
        }
    }
    
    void OnEnable(){
        Debug.Log("Starting by BuildingsManager");
        //MenuNetwork.Start(ConnectionAdded);
    }
    

    void Update(){
        
        MenuNetwork.Update();
    }
    /*
    public void ConnectionAdded(IConnection connection) {
        connection.AddListener<ServerStartLobbyGameAction>(action => {
            StartGame()
        });
    }
*/
    //___________________________________________________________//
    //___________________________________________________________//
    //___________________________________________________________//

    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/BuildingsScene");
    }
}