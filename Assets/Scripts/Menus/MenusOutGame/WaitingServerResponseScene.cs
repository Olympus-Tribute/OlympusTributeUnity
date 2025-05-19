using ForNetwork;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus.MenusOutGame
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
                SteamFriends.ActivateGameOverlay("Friends");
            }
        }
        
        //___________________________________________________________//
        //___________________________________________________________//
        //___________________________________________________________//
    }
}