using System.Collections.Generic;
using ForNetwork;
using Networking.API.Listeners;
using Networking.Common.Client;
using Networking.Common.Server;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menus.MenusOutGame
{
    public class WaitingScene : MonoBehaviour
    {
        private bool[] _readyStates;
    
        public GameObject imageReadyPlayer1;
        public GameObject imageNotReadyPlayer1;
        public GameObject imageFlagPlayer1;
        public GameObject nameFlagPlayer1;
    
        public GameObject imageReadyPlayer2;
        public GameObject imageNotReadyPlayer2;
        public GameObject imageFlagPlayer2;
        public GameObject nameFlagPlayer2;
    
        public GameObject imageReadyPlayer3;
        public GameObject imageNotReadyPlayer3;
        public GameObject imageFlagPlayer3;
        public GameObject nameFlagPlayer3;
    
        public GameObject imageReadyPlayer4;
        public GameObject imageNotReadyPlayer4;
        public GameObject imageFlagPlayer4;
        public GameObject nameFlagPlayer4;
    
        public bool playerReady;

        public GameObject buttonReady;
        public GameObject buttonNotReady;
        
        public GameActionListener<ServerReadyStatesGameAction> readyActionListener;
        public GameActionListener<ServerStartLobbyGameAction> lobbyActionListener;
        
        //___________________________________________________________//
        //___________________________________________________________//
        //___________________________________________________________//
        
        public enum TypeOfAi
        {
            Random,
            Backtrack,
            BactrackMultiworld
        }
        
        public Dictionary<TypeOfAi, int> AiToAddGame = new Dictionary<TypeOfAi, int>();
        [SerializeField] public GameObject menuForHost;
        [SerializeField] public TMP_Dropdown aiDropdown;
        
        //___________________________________________________________//
        //_________________________For Multi_________________________//
        //___________________________________________________________//
    
        void OnEnable()
        {
            RegisterGameAction(Network.Instance.Proxy);
            
            buttonReady.SetActive(true);
            buttonNotReady.SetActive(false);
            SetAllActiveFalse();
            playerReady = false;
            _readyStates = new[] { false };
            UpdatePlayersReady();

            menuForHost.SetActive(LocalConnectionMethod.Instance.PlayerIsHost);
        }

        public void OnDisable()
        {
            if (Network.Instance.Proxy != null)
            {
                Network.Instance.Proxy.GameActionListenerManager.RemoveListener(readyActionListener);
                Network.Instance.Proxy.GameActionListenerManager.RemoveListener(lobbyActionListener);
            }
        }

        private void RegisterGameAction(Proxy proxy)
        {
            readyActionListener = proxy.GameActionListenerManager.AddListener<ServerReadyStatesGameAction>(
                (connection, action) =>
                {
                    _readyStates = action.ReadyStates;
                    UpdatePlayersReady();
                });


            lobbyActionListener = proxy.GameActionListenerManager.AddListener<ServerStartLobbyGameAction>(
                (connection, action) =>
                {
                    Debug.Log("[CLIENT]     : Received ServerStartLobbyGameAction");
                    GameConstants.Seed = (int)action.Seed;
                    GameConstants.MapWidth = action.Width;
                    GameConstants.MapHeight = action.Height;
                    GameConstants.PlayerCount = action.PlayerCount;
                    GameConstants.PlayerId = action.PlayerId;
                    InitGame();
                });
        }

        //___________________________________________________________//
        //___________________________________________________________//
        //___________________________________________________________//
        
        public void Update()
        {
            UpdatePlayersReady();
        }

        public void SetAllActiveFalse()
        {
            imageReadyPlayer1.SetActive(true);
            imageNotReadyPlayer1.SetActive(false);
            imageFlagPlayer1.SetActive(true);
            nameFlagPlayer1.SetActive(true);
        
        
            imageReadyPlayer2.SetActive(false);
            imageNotReadyPlayer2.SetActive(false);
            imageFlagPlayer2.SetActive(false);
            nameFlagPlayer2.SetActive(false);
        
            imageReadyPlayer3.SetActive(false);
            imageNotReadyPlayer3.SetActive(false);
            imageFlagPlayer3.SetActive(false);
            nameFlagPlayer3.SetActive(false);
        
            imageReadyPlayer4.SetActive(false);
            imageNotReadyPlayer4.SetActive(false);
            imageFlagPlayer4.SetActive(false);
            nameFlagPlayer4.SetActive(false);
        }
   

        public void Ready() // For Button
        {
            playerReady = true;
            buttonReady.SetActive(false);
            buttonNotReady.SetActive(true);
            Network.Instance.Proxy.Connection.Send(new ClientReadyStateGameAction(true));
            Debug.Log("Envoi du gameAction pour Ready");
        }
    
        public void NotReady() // For Button
        {
            playerReady = false;
            buttonReady.SetActive(true);
            buttonNotReady.SetActive(false);
            Network.Instance.Proxy.Connection.Send(new ClientReadyStateGameAction(false));
            Debug.Log("Envoi du gameAction pour Not Ready");
        }
    
        public void BackToMainMenu()
        {
            SetAllActiveFalse();
            SceneManager.LoadScene("MainMenu");
            Network.Instance.Stop();
        }
    
        public void InitGame()
        {
            SceneManager.LoadScene("PlayGame");
        }
    
        public void UpdatePlayersReady()
        {
            for (int i = 0; i < _readyStates.Length; i++)
            {
                bool ready = _readyStates[i];
                switch (i)
                {
                    case 0 when ready:
                        imageReadyPlayer1.SetActive(true);
                        imageNotReadyPlayer1.SetActive(false);
                        imageFlagPlayer1.SetActive(true);
                        nameFlagPlayer1.SetActive(true);
                        break;
                    case 0:
                        imageReadyPlayer1.SetActive(false);
                        imageNotReadyPlayer1.SetActive(true);
                        imageFlagPlayer1.SetActive(true);
                        nameFlagPlayer1.SetActive(true);
                        break;
                    case 1 when ready:
                        imageReadyPlayer2.SetActive(true);
                        imageNotReadyPlayer2.SetActive(false);
                        imageFlagPlayer2.SetActive(true);
                        nameFlagPlayer2.SetActive(true);
                        break;
                    case 1:
                        imageReadyPlayer2.SetActive(false);
                        imageNotReadyPlayer2.SetActive(true);
                        imageFlagPlayer2.SetActive(true);
                        nameFlagPlayer2.SetActive(true);
                        break;
                    case 2 when ready:
                        imageReadyPlayer3.SetActive(true);
                        imageNotReadyPlayer3.SetActive(false);
                        imageFlagPlayer3.SetActive(true);
                        nameFlagPlayer3.SetActive(true);
                        break;
                    case 2:
                        imageReadyPlayer3.SetActive(false);
                        imageNotReadyPlayer3.SetActive(true);
                        imageFlagPlayer3.SetActive(true);
                        nameFlagPlayer3.SetActive(true);
                        break;
                    case 3 when ready:
                        imageReadyPlayer4.SetActive(true);
                        imageNotReadyPlayer4.SetActive(false);
                        imageFlagPlayer4.SetActive(true);
                        nameFlagPlayer4.SetActive(true);
                        break;
                    case 3:
                        imageReadyPlayer4.SetActive(false);
                        imageNotReadyPlayer4.SetActive(true);
                        imageFlagPlayer4.SetActive(true);
                        nameFlagPlayer4.SetActive(true);
                        break;
                }
            }
        }
        
        //___________________________________________________________//
        //_____________________Added_AI______________________________//
        //___________________________________________________________//

        private TypeOfAi ConvertIntToTypeOfAi(int value)
        {
            switch (value)
            {
                case 0: return TypeOfAi.Random;
                case 1: return TypeOfAi.Backtrack;
                case 2: return TypeOfAi.BactrackMultiworld;
                default: return TypeOfAi.Random;
            }
        }
        
        public void AddAi()
        {
            TypeOfAi typeOfAi = ConvertIntToTypeOfAi(aiDropdown.value);
            if (AiToAddGame.TryGetValue(typeOfAi, out int _))
            {
                AiToAddGame[typeOfAi] += 1;
            }
            else
            {
                AiToAddGame.Add(typeOfAi, 1);
            }
            Debug.Log($"Added {typeOfAi} | {AiToAddGame[typeOfAi]} Ai in this Game");
        }
        
        public void RemoveAi()
        {
            TypeOfAi typeOfAi = ConvertIntToTypeOfAi(aiDropdown.value);
            if (AiToAddGame.TryGetValue(typeOfAi, out int _))
            {
                AiToAddGame[typeOfAi] -= 1;
                Debug.Log($"Remove {typeOfAi} | {AiToAddGame[typeOfAi]} Ai in this Game");
                if (AiToAddGame[typeOfAi] <= 0)
                {
                    AiToAddGame.Remove(typeOfAi);
                }
            }
        }
        
        //___________________________________________________________//
        //___________________________________________________________//
        //___________________________________________________________//
    }
}