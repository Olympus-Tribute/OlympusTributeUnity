using System.IO;
using ForNetwork;
using PopUp;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menus.MenusOutGame
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] public GameObject menuUIMainMenu;
        [SerializeField] public GameObject menuUIOptionsMenu;
        [SerializeField] public GameObject menuUIMultiplayerOrAi;
        
        [SerializeField] public GameObject menuUIMultiplayerHostOrJoinMenu;
        [SerializeField] public GameObject menuUIMultiplayerHost;
        [SerializeField] public GameObject menuUIMultiplayerJoin;
        
        [SerializeField] public GameObject menuUIMultiplayerSelectSteamOrTcp;


        [SerializeField] public Toggle toggleSteam;
        [SerializeField] public InputField inputIpAdressField;
        
        public bool acceptSteam = false;
        public bool acceptTcp = true;
        public bool creativeMode = false;
        
        [SerializeField] private GameObject tcpConnectionPrefab;
        [SerializeField] private GameObject steamConnectionPrefab;
        [SerializeField] private GameObject localConnectionPrefab;

        private void OnEnable()
        {
            CreateSteamAppIdFile();
            SetAllMenusInactive();
            menuUIMainMenu.SetActive(true);
            
            Instantiate(tcpConnectionPrefab);
            Instantiate(localConnectionPrefab);

            if (SteamManager.Initialized)
            {
                Instantiate(steamConnectionPrefab);
                acceptSteam = true;
                toggleSteam.isOn = true;
            }
            else
            {
                acceptSteam = false;
                toggleSteam.isOn = false;
            }
        }

        private void SetAllMenusInactive()
        {
            menuUIMainMenu.SetActive(false);
            menuUIOptionsMenu.SetActive(false);
            menuUIMultiplayerOrAi.SetActive(false);
            
            menuUIMultiplayerHostOrJoinMenu.SetActive(false);
            menuUIMultiplayerHost.SetActive(false);
            menuUIMultiplayerJoin.SetActive(false);
            
            menuUIMultiplayerSelectSteamOrTcp.SetActive(false);
        }
        
        public void BackToMainMenu()
        {
            SetAllMenusInactive();
            menuUIMainMenu.SetActive(true);
        }
        
        private static void CreateSteamAppIdFile()
        {
            string filePath = "steam_appid.txt";
        
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "480");
            
            }
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
            menuUIMainMenu.SetActive(true);
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
    
        public void SelectHost()
        {
            SetAllMenusInactive();
            menuUIMultiplayerHost.SetActive(true);
        }
    
        public void SelectJoin()
        {
            SetAllMenusInactive();
            menuUIMultiplayerJoin.SetActive(true);
        }
        
        //__________________________________________________________//
        //_____________MenuSelectTypeOfMultiJoin____________________//
        //__________________________________________________________//


        public void JoinWithSteam()
        {
            OpenFriendsOverlay();
        }
        
        private void OpenFriendsOverlay()
        {
            if (SteamManager.Initialized)
            {
                SteamFriends.ActivateGameOverlay("Friends");
                SceneManager.LoadScene("Scenes/Menus/WaitingServerResponseScene");
            }
            else
            {
                PopUpManager.Instance.ShowPopUp("Steam is not initialized or not running.", 1);
                Debug.LogError("Steam is not initialized or not running.");
            }
        }
        
        public void JoinWithTcp()
        {
            string ipText = inputIpAdressField.text;
            
            OpenFriendsOverlay();
        }
        
        //__________________________________________________________//
        //_____________MenuHost_____________________________________//
        //__________________________________________________________//


        public void ToggleAcceptSteam()
        {
            acceptSteam = !acceptSteam;
            if (acceptSteam)
            {
                Debug.Log("ToggleAcceptSteam");
            }
            else
            {
                Debug.Log("ToggleAcceptSteam false");
            }
            
        }
        
        public void ToggleAcceptTcp()
        {
            acceptTcp = !acceptTcp;
            if (acceptTcp)
            {
                Debug.Log("ToggleAcceptTcp");
            }
            else
            {
                Debug.Log("ToggleAcceptTCP false");
            }
            
        }
        
        public void ToggleCreativeMode()
        {
            creativeMode = !creativeMode;
            if (creativeMode)
            {
                Debug.Log("creativeMode");
            }
            else
            {
                Debug.Log("creativeMode false");
            }
        }
        
        //____________________________________
        
        public void MenuHost()
        {
            SetAllMenusInactive();
            
            LocalConnectionMethod.Instance.acceptSteamConnection = acceptSteam;
            LocalConnectionMethod.Instance.acceptTcpConnection = acceptTcp;
            
            LocalConnectionMethod.Instance.Connect();
        }
    }
}