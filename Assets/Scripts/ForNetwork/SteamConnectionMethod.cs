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
        private Callback<LobbyEnter_t> callbackLobbyEnter_t;
        private Callback<LobbyChatUpdate_t> callbackLobbyChatUpdate_t;
        
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
                callbackLobbyChatUpdate_t = Callback<LobbyChatUpdate_t>.Create(LobbyChatUpdate);
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
                    Debug.Log("Connecting via Steam");
                    
                    SteamConnection steamConnection = SteamConnection.Connect(_hostID.Value, registry);
               
                    GameActionListenerManager gameActionListener = new GameActionListenerManager();
 
                    FinaliseConnection(new Proxy(steamConnection, gameActionListener));

                    _hostID = null;
                }
            }
        }

        public override void Stop()
        {
            _hostID = null;
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
        
        private void LobbyChatUpdate(LobbyChatUpdate_t param)
        {
            if (param.m_ulSteamIDUserChanged != SteamUser.GetSteamID().m_SteamID || (param.m_rgfChatMemberStateChange ==
                    (uint)EChatMemberStateChange.k_EChatMemberStateChangeEntered)) return;
            
            _lobbyID = null;
            _hostID = null;
        }
    }
}