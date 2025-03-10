using ForNetwork;
using ForServer;
using Networking.Common.Client;
using Networking.Common.Server;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
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

        public GameObject hideButtonReady;
        public GameObject hideButtonNotReady;
        //___________________________________________________________//
        //_________________________For Multi_________________________//
        //___________________________________________________________//
    
        void OnEnable()
        {
            if (Network.Instance.Proxy is not null)
            {
                RegisterGameAction(Network.Instance.Proxy);
            }
            else
            {
                Network.Instance.Setup = (proxy => { RegisterGameAction(proxy); });
            }
        }

        private void RegisterGameAction(Proxy proxy)
        {
            proxy.GameActionListenerManager.AddListener<ServerReadyStatesGameAction>(
                (connection, action) =>
                {
                    _readyStates = action.ReadyStates;
                    UpdatePlayersReady();
                });


            proxy.GameActionListenerManager.AddListener<ServerStartLobbyGameAction>(
                (connection, action) =>
                {
                    Debug.Log("[CLIENT]     : Received ServerStartLobbyGameAction");
                    ServerManager.Seed = (int)action.Seed;
                    ServerManager.MapWidth = action.Width;
                    ServerManager.MapHeight = action.Height;
                    ServerManager.PlayerCount = action.PlayerCount;
                    ServerManager.PlayerId = action.PlayerId;
                    InitGame();
                });
        }

        //___________________________________________________________//
        //___________________________________________________________//
        //___________________________________________________________//

        public void Start()
        {
            hideButtonReady.SetActive(false);
            hideButtonNotReady.SetActive(true);
            SetAllActiveFalse();
            playerReady = false;
            _readyStates = new[] { false };
            UpdatePlayersReady();
        }

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
            hideButtonReady.SetActive(true);
            hideButtonNotReady.SetActive(false);
            Network.Instance.Proxy.Connection.Send(new ClientReadyStateGameAction(true));
            Debug.Log("Envoi du gameAction pour Ready");
        }
    
        public void NotReady() // For Button
        {
            playerReady = false;
            hideButtonReady.SetActive(false);
            hideButtonNotReady.SetActive(true);
            Network.Instance.Proxy.Connection.Send(new ClientReadyStateGameAction(false));
            Debug.Log("Envoi du gameAction pour Not Ready");
        }
    
        public void BackToMainMenu()
        {
            SetAllActiveFalse();
            SceneManager.LoadScene("MainMenu");
            Network.Instance.Proxy.Connection.Disconnect();
        }
    
        public void InitGame()
        {
            SceneManager.LoadScene("PlayGame");
        }
    
        //___________________________________________________________//
        //___________________________________________________________//
        //___________________________________________________________//
    
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
    }
}