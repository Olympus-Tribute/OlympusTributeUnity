using System;
using JetBrains.Annotations;
using Networking.API.Actions;
using Networking.API.Listeners;
using Networking.Common.Client;
using Networking.Common.Server;
using Networking.Steam;
using Steamworks;
using UnityEngine;

namespace ForNetwork
{
    public class Network : MonoBehaviour
    {
        private CSteamID? _lockObject;
        [CanBeNull] public Proxy Proxy; 
        [CanBeNull] public Action<Proxy> Setup;
        
        private Callback<GameRichPresenceJoinRequested_t> callbackGameRichPresenceJoinRequested_t;
        private Callback<GameLobbyJoinRequested_t> callbackGameLobbyJoinRequested_t;
        private Callback<LobbyChatUpdate_t> callbackLobbyChatUpdate_t;
        private Callback<LobbyEnter_t> callbackLobbyEnter_t;
        
        public static Network Instance {get; private set;}
        public bool networkActive;

        private Network()
        {
            Proxy = null;
        }

        public void Awake()
        {
            networkActive = false;
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Optionnel, pour garder l'instance entre les scènes
            }
            else
            {
                Destroy(gameObject); // Évite les doublons
            }
        }

        public void Start()
        {
            Debug.Log("Start Network");
            SetupCallbacks();
            Debug.Log("Setup callbacks");
        
        }

        public void Update()
        {
            if (Proxy == null){
                
                if (!_lockObject.HasValue)
                {
                    return;
                }
                
                var registry = new GameActionRegistry();
                registry.Register<ClientWantsStartGameAction>();
                registry.Register<ServerStartLobbyGameAction>();
                registry.Register<ClientPlaceBuildingGameAction>();
                registry.Register<ServerPlaceBuildingGameAction>();

                SteamConnection steamConnection = SteamConnection.Connect(_lockObject.Value, registry);
                GameActionListenerManager gameActionListener = new GameActionListenerManager();
                Proxy = new Proxy(steamConnection, gameActionListener);
                    
                Debug.Log("Created connection");
                Proxy.Connection.Connect();
                Debug.Log("Connected");

                Setup(Proxy);
                return;
            }
            Proxy.Process();
        }
        
        
        
        private void SetupCallbacks()
        {
            callbackGameRichPresenceJoinRequested_t = Callback<GameRichPresenceJoinRequested_t>.Create(JoinRequestRichPresence);
            callbackGameLobbyJoinRequested_t = Callback<GameLobbyJoinRequested_t>.Create(JoinRequestLobby);
            callbackLobbyChatUpdate_t = Callback<LobbyChatUpdate_t>.Create(param =>
            {
                Debug.Log("Lobby join requested " + param.m_ulSteamIDLobby);

                SteamFriends.SetRichPresence("steam_player_group", param.m_ulSteamIDLobby.ToString());
                SteamFriends.SetRichPresence("steam_player_group_size",
                    SteamMatchmaking.GetNumLobbyMembers(new CSteamID(param.m_ulSteamIDLobby)).ToString());
            });
            callbackLobbyEnter_t = Callback<LobbyEnter_t>.Create(LobbyJoined);
        }

        private void LobbyJoined(LobbyEnter_t param)
        {
            Debug.Log("Joining, create connection");
            var hostId = SteamMatchmaking.GetLobbyData(new CSteamID(param.m_ulSteamIDLobby), "host_id");
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

        private bool Init()
        {
            if (!Packsize.Test())
                Debug.Log(
                    "[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");

            if (!DllCheck.Test())
                Debug.Log(
                    "[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
            
            if (!SteamAPI.Init())
            {
                Debug.Log("Could not initialise steam api");
                return true;
            }
            SteamNetworkingUtils.InitRelayNetworkAccess();
            return false;
        }
    }
}