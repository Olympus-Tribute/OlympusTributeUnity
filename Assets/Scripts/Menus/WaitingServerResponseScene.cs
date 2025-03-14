using ForNetwork;
using ForServer;
using Networking.Common.Server;
using PopUp;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class WaitingServerResponseScene : MonoBehaviour
    {
        //___________________________________________________________//
        //_________________________For Multi_________________________//
        //___________________________________________________________//
    
        void OnEnable()
        {
            
            if (Network.Instance.Proxy is not null)
            {
                SceneManager.LoadScene("WaitingScene");
            }
            else
            {
                Network.Instance.Setup = (proxy =>
                {
                    SceneManager.LoadScene("WaitingScene");
                });
                SteamFriends.ActivateGameOverlay("Friends");
            }
        }
        
        //___________________________________________________________//
        //___________________________________________________________//
        //___________________________________________________________//
    }
}