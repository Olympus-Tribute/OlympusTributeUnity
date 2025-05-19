using System;
using JetBrains.Annotations;
using Microsoft.Win32;
using Networking.API.Actions;
using Networking.API.Listeners;
using Networking.Common.Client;
using Networking.Common.Server;
using Networking.Steam;
using Networking.TCP;
using OlympusDedicatedServer;
using Steamworks;
using UnityEngine;

namespace ForNetwork
{
    public class Network : MonoBehaviour
    {
        private CSteamID? _lockObject;
        public Proxy Proxy; 
        public Action<Proxy> Setup;
        
        private Callback<GameRichPresenceJoinRequested_t> callbackGameRichPresenceJoinRequested_t;
        private Callback<GameLobbyJoinRequested_t> callbackGameLobbyJoinRequested_t;
        private Callback<LobbyChatUpdate_t> callbackLobbyChatUpdate_t;
        private Callback<LobbyEnter_t> callbackLobbyEnter_t;
        
        public static Network Instance {get; private set;}
        public GameActionRegistry registry;
        private CSteamID? _lobbyID;

        private Network()
        {
            Proxy = null;
            Setup = (proxy => {});
            
            // Register des GameAction
            registry = GameActions.RegisterAll();
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Optionnel, pour garder l'instance entre les scènes
                //SetupCallbacks();
            }
            else
            {
                Destroy(gameObject); // Évite les doublons
            }
            
        }

        public void Update()
        {
            if (Proxy == null){
                Console.SetOut(new MyDebug());
                
                /*
                if (!_lockObject.HasValue)
                {
                    return; 
                }
                */

                //SteamConnection steamConnection = SteamConnection.Connect(_lockObject.Value, registry);
                TcpConnection connection = TcpConnection.Connect("82.67.95.235", 12345, registry);
                GameActionListenerManager gameActionListener = new GameActionListenerManager();
                Proxy = new Proxy(connection, gameActionListener);
                    
                Debug.Log("Created connection");
                Proxy.Connection.Connect();
                Debug.Log("Connected");

                Setup(Proxy);
                
                _lockObject = null;
                
                return;
            }
            Proxy.Process();
        }

        public void Stop()
        {
            if (_lobbyID != null)
            {
                //SteamMatchmaking.LeaveLobby(_lobbyID.Value);
            }
            _lobbyID = null;
        }
        
        
        
        private void SetupCallbacks()
        {
            callbackGameRichPresenceJoinRequested_t = Callback<GameRichPresenceJoinRequested_t>.Create(JoinRequestRichPresence);
            callbackGameLobbyJoinRequested_t = Callback<GameLobbyJoinRequested_t>.Create(JoinRequestLobby);
            callbackLobbyEnter_t = Callback<LobbyEnter_t>.Create(LobbyJoined);
        }

        private void LobbyJoined(LobbyEnter_t param)
        {
            Debug.Log("Joining, create connection");
            
            _lobbyID = new CSteamID(param.m_ulSteamIDLobby);
            
            var hostId = SteamMatchmaking.GetLobbyData(_lobbyID.Value, "host_id");
            _lockObject = new CSteamID(ulong.Parse(hostId));
        }

        private void JoinRequestLobby(GameLobbyJoinRequested_t param)
        {
            Debug.Log("Joining lobby via request");
            SteamMatchmaking.JoinLobby(param.m_steamIDLobby);
        }

        private void JoinRequestRichPresence(GameRichPresenceJoinRequested_t param)
        {
            Debug.Log("Joining lobby via rich presence");
            SteamMatchmaking.JoinLobby(new CSteamID(ulong.Parse(param.m_rgchConnect)));
        }
    }
}