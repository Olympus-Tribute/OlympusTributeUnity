using System;
using JetBrains.Annotations;
using Networking.API.Actions;
using Networking.API.Listeners;
using Networking.Steam;
using Steamworks;
using UnityEngine;

namespace ForNetwork
{
    public class SteamConnectionMethod : ConnectionMethod
    {
        private CSteamID? _hostID;
        
        private Callback<GameRichPresenceJoinRequested_t> callbackGameRichPresenceJoinRequested_t;
        private Callback<GameLobbyJoinRequested_t> callbackGameLobbyJoinRequested_t;
        private Callback<LobbyChatUpdate_t> callbackLobbyChatUpdate_t;
        private Callback<LobbyEnter_t> callbackLobbyEnter_t;
        
        [CanBeNull] public static SteamConnectionMethod Instance { get; private set; }
        
        private CSteamID? _lobbyID;
        
        public SteamConnectionMethod(){}
        
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                callbackGameRichPresenceJoinRequested_t = Callback<GameRichPresenceJoinRequested_t>.Create(JoinRequestRichPresence);
                callbackGameLobbyJoinRequested_t = Callback<GameLobbyJoinRequested_t>.Create(JoinRequestLobby);
                callbackLobbyEnter_t = Callback<LobbyEnter_t>.Create(LobbyJoined);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void Update()
        {
            Connect();
        }

        public override void Connect()
        {
            if (Network.Instance.Proxy == null)
            {
                if (_hostID != null)
                {
                    SteamConnection steamConnection = SteamConnection.Connect(_hostID.Value, registry);
               
                    GameActionListenerManager gameActionListener = new GameActionListenerManager();
 
                    FinaliseConnection(new Proxy(steamConnection, gameActionListener));

                    _hostID = null;
                }
            }
        }

        public override void Stop()
        {
            if (_lobbyID != null)
            {
                SteamMatchmaking.LeaveLobby(_lobbyID.Value);
            }
            _lobbyID = null;
        }

        private void LobbyJoined(LobbyEnter_t param)
        {
            Debug.Log("Joining, create connection");
            
            _lobbyID = new CSteamID(param.m_ulSteamIDLobby);
            
            var hostId = SteamMatchmaking.GetLobbyData(_lobbyID.Value, "host_id");
            _hostID = new CSteamID(ulong.Parse(hostId));
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