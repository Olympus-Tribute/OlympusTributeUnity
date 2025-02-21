using System;
using ForServer;
using PopUp;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject menuUIMainMenu;
        public GameObject menuUIOptionsMenu;
        public GameObject menuUIMultiplayerOrAi;
        //public GameObject menuUIMultiplayerSteamOrTcpIp;
        public GameObject menuUIMultiplayerHostOrJoinMenu;
        

        private void Start()
        {
            SetAllMenusInactive();
            menuUIMainMenu.SetActive(true);
        }

        private void SetAllMenusInactive()
        {
            menuUIMainMenu.SetActive(false);
            menuUIOptionsMenu.SetActive(false);
            menuUIMultiplayerOrAi.SetActive(false);
            //menuUIMultiplayerSteamOrTcpIp.SetActive(false);
            menuUIMultiplayerHostOrJoinMenu.SetActive(false);
        }
        
        public void BackToMainMenu()
        {
            SetAllMenusInactive();
            menuUIMainMenu.SetActive(true);
        }

        //__________________________________________________________//
        //________________________MainMenu__________________________//
        //__________________________________________________________//
        
        public void PlayGame()
        {
            SetAllMenusInactive();
            menuUIMultiplayerOrAi.SetActive(true);
        }
        public void OpenOptions()
        {
            SetAllMenusInactive();
            menuUIOptionsMenu.SetActive(true);
        }
        public void QuitGame()
        {
            Debug.Log("Quitter le jeu !");
            Application.Quit();
        }
        
        //__________________________________________________________//
        //________________________OptionMenu________________________//
        //__________________________________________________________//
        
        public void Option1()
        {
            Debug.Log("Option1");
        }
    
        public void Option2()
        {
            Debug.Log("Option2");
        }

        public void Option3()
        {
            Debug.Log("Option3");
        }
        
        //__________________________________________________________//
        //________________________MultiplayerOrAi___________________//
        //__________________________________________________________//

        
        public void PlayOnMultiplayer()
        {
            SetAllMenusInactive();
            menuUIMultiplayerHostOrJoinMenu.SetActive(true);
        }
    
        public void PlayWithAI() // TODO
        {
            SetAllMenusInactive();
            menuUIMultiplayerHostOrJoinMenu.SetActive(true);
        }
        
        //__________________________________________________________//
        //________________________HostOrJoin________________________//
        //__________________________________________________________//
        
        private void OpenFriendsOverlay()
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
            if (SteamManager.Initialized)
            {
                SetAllMenusInactive();
                
                CSteamID id = SteamUser.GetSteamID();
                //SteamFriends.ActivateGameOverlayInviteDialog(id);
        
                ServerManager.Instance.Host();
        
                SceneManager.LoadScene("WaitingScene");
            }
            else
            {
                Debug.LogError("Steam is not initialized or not running.");
            }
            
        }
    
        public void Join()
        {
            OpenFriendsOverlay();
        }
    }
}