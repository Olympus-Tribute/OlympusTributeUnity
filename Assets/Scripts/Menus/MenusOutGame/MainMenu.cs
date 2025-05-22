using System;
using System.IO;
using ForNetwork;
using PopUp;
using Steamworks;
using TMPro;
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
        
        //_______________________________________________________//
        //_______________________________________________________//
        
        [SerializeField] public Toggle toggleSteam;
        [SerializeField] public Toggle toggleTcp;
        [SerializeField] public Toggle toggleCreativeMode;
        [SerializeField] public TMP_InputField inputIpAdressField;
        
        public bool acceptSteam;
        public bool acceptTcp;
        public bool creativeModeIsActive;
        
        [SerializeField] private GameObject tcpConnectionPrefab;
        [SerializeField] private GameObject steamConnectionPrefab;
        [SerializeField] private GameObject localConnectionPrefab;
        
        [SerializeField] private GameObject gameObjectToggleSteamWithTmp;
        
        [SerializeField] public Toggle toggleTimer;
        [SerializeField] public Toggle togglePercentage;
        
        [SerializeField] public GameObject gameObjectPercentage;
        [SerializeField] public TMP_InputField percentageInputField;
        
        [SerializeField] public GameObject gameObjectTimer;
        [SerializeField] public TMP_InputField timerInputField;
        
        private void OnEnable()
        {
            CreateSteamAppIdFile();
            SetAllMenusInactive();
            menuUIMainMenu.SetActive(true);
            
            Instantiate(tcpConnectionPrefab);
            Instantiate(localConnectionPrefab);
            
            //_____________________//
            
            acceptTcp = true;
            acceptSteam = false;
            
            if (SteamManager.Initialized)
            {
                Instantiate(steamConnectionPrefab);
                acceptSteam = true;
                gameObjectToggleSteamWithTmp.SetActive(true);
            }
            else
            {
                gameObjectToggleSteamWithTmp.SetActive(false);
            }
            
            toggleSteam.isOn = acceptSteam;
            toggleTcp.isOn = acceptTcp;
            
            //_____________________//
            
            creativeModeIsActive = false;
            toggleCreativeMode.isOn = creativeModeIsActive;
            
            //_____________________//

            GameConstants.TimerModeIsActive = true;
            GameConstants.PercentageModeSelected = false;

            GameConstants.PercentageSetByHost = 100;
            GameConstants.TimerSetByHostInMin = 1140;

            toggleTimer.isOn = GameConstants.TimerModeIsActive;
            gameObjectTimer.SetActive(toggleTimer.isOn);
            
            togglePercentage.isOn = GameConstants.PercentageModeSelected;
            gameObjectPercentage.SetActive(togglePercentage.isOn);
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
    
        public void PlayWithAI()
        {
            SetAllMenusInactive();
            menuUIMultiplayerHost.SetActive(true);
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
            Debug.Log($"Ip Adress : {inputIpAdressField.text}");
            TcpConnectionMethod.Instance.hostIP = inputIpAdressField.text;
            TcpConnectionMethod.Instance.Connect();

            if (Network.Instance.Proxy is null)
            {
                PopUpManager.Instance.ShowPopUp("Invalid IP", 3);
            }
        }
        
        //__________________________________________________________//
        //_____________MenuHost_____________________________________//
        //__________________________________________________________//


        public void ToggleAcceptSteam()
        {
            acceptSteam = !acceptSteam;
            Debug.Log(acceptSteam ? "ToggleAcceptSteam" : "ToggleAcceptSteam false");
        }
        
        public void ToggleAcceptTcp()
        {
            acceptTcp = !acceptTcp;
            Debug.Log(acceptTcp ? "ToggleAcceptTcp" : "ToggleAcceptTCP false");
        }
        
        public void ToggleCreativeMode()
        {
            creativeModeIsActive = !creativeModeIsActive;
        }
        
        public void ToggleTimerModeSelected()
        {
            GameConstants.TimerModeIsActive = !GameConstants.TimerModeIsActive;
            VerifToggleWinCondition();
            gameObjectTimer.SetActive(GameConstants.TimerModeIsActive);
        }
        
        public void InputTimer()
        {
            if (Int32.TryParse(timerInputField.text, out int timerInput))
            {
                if (0 <= timerInput && timerInput <= 1440)
                {
                    GameConstants.TimerSetByHostInMin = timerInput;
                    Debug.Log($"Timer Input : {timerInput} min");
                    return;
                }
            }
            PopUpManager.Instance.ShowPopUp("Invalid Timer", 2);
        }
        
        public void TogglePercentageModeSelected()
        {
            GameConstants.PercentageModeSelected = !GameConstants.PercentageModeSelected;
            VerifToggleWinCondition();
            gameObjectPercentage.SetActive(GameConstants.PercentageModeSelected);
        }
        
        public void InputPercentage()
        {
            if (Int32.TryParse(percentageInputField.text, out int percentageInput))
            {
                if (0 <= percentageInput && percentageInput <= 100)
                {
                    GameConstants.PercentageSetByHost = (uint)percentageInput;
                    Debug.Log($"Percentage Input : {percentageInput}%");
                    return;
                }
            }
            PopUpManager.Instance.ShowPopUp("Invalid Percentage", 2);
        }
        
        private void VerifToggleWinCondition()
        {
            if (GameConstants.TimerModeIsActive || GameConstants.PercentageModeSelected) return;
            toggleTimer.isOn = true;
            gameObjectTimer.SetActive(toggleTimer.isOn);
            
            togglePercentage.isOn = false;
            gameObjectPercentage.SetActive(togglePercentage.isOn);
                
            PopUpManager.Instance.ShowPopUp("Please select at least one win condition method", 2);
        }
        
        public void MenuHost()
        {
            SetAllMenusInactive();
            
            LocalConnectionMethod.Instance.acceptSteamConnection = acceptSteam;
            LocalConnectionMethod.Instance.acceptTcpConnection = acceptTcp;
            LocalConnectionMethod.Instance.creativeModeIsActive = creativeModeIsActive;
            
            LocalConnectionMethod.Instance.Connect();
        }
    }
}