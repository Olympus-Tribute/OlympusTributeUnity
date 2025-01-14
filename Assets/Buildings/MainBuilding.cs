using System;
using Networking.API;
using Networking.API.Actions;
using Networking.Common.Client;
using Networking.Common.Server;
using Networking.Steam;
using Steamworks;
using UnityEngine;

public class MainBuilding
{
    private CSteamID? _lockObject;
    public IConnection Connection; 
    private Action<IConnection> _setup;
    
    private Callback<GameRichPresenceJoinRequested_t> callbackGameRichPresenceJoinRequested_t;
    private Callback<GameLobbyJoinRequested_t> callbackGameLobbyJoinRequested_t;
    private Callback<LobbyChatUpdate_t> callbackLobbyChatUpdate_t;
    private Callback<LobbyEnter_t> callbackLobbyEnter_t;

    public MainBuilding()
    {
        Connection = null;
    }

    public void Start(Action<IConnection> setup)
    {
        Debug.Log("Start Methode MainBuilding");
        _setup = setup;
        
        SetupCallbacks();
        Debug.Log("Setup callbacks");
    
    }

    public void Update()
    {
        if (Connection == null){
            
            if (!_lockObject.HasValue) return;
            
            var registry = new GameActionRegistry();
            registry.Register<ClientPlaceBuildingGameAction>();
            registry.Register<ServerPlaceBuildingGameAction>();
                
            Connection = SteamConnection.Connect(_lockObject.Value, registry);
                
            Debug.Log("Created connection");

            Connection.Connect();
            Debug.Log("Connected");

            _setup(Connection);
            return;
        }
        Connection.Update();
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
        /*
        try
        {
            if (SteamAPI.RestartAppIfNecessary((AppId_t)480))
            {
                return true;
            }
        }

        catch (DllNotFoundException e)
        {
            Debug.Log(
                "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" +
                e);

            return true;
        }
        */
        if (!SteamAPI.Init())
        {
            Debug.Log("Could not initialise steam api");
            return true;
        }
        SteamNetworkingUtils.InitRelayNetworkAccess();
        return false;
    }
}