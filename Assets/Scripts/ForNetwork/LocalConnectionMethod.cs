using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Networking.API;
using Networking.API.Actions;
using Networking.API.Listeners;
using Networking.Local;
using Networking.Steam;
using Networking.TCP;
using OlympusDedicatedServer;
using OlympusDedicatedServer.Components.WorldComp.Win;
using Steamworks;
using UnityEngine;

namespace ForNetwork
{
    public class LocalConnectionMethod : ConnectionMethod
    {
        private CSteamID? _lobbyID;
        public static LocalConnectionMethod Instance { get; private set; }

        public bool acceptTcpConnection;
        public bool acceptSteamConnection;
        public bool creativeModeIsActive;
        
        public Server Server { get; set; }

        public bool PlayerIsHost => Server != null;

        public LocalConnectionMethod(){}

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                if (SteamManager.Initialized)
                {
                    SetupCallbacks();
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Update()
        {
            Server?.Update(Time.deltaTime);
        }

        public override void Connect()
        {
            Debug.Log("Hosting Server....");
            Console.SetOut(new MyDebug());
            
            List<IConnectionAcceptor> acceptors = new();
            
            LocalConnectionAcceptor localAcceptor = new();
            
            acceptors.Add(localAcceptor);
            Add(acceptors);
            
            CompositionConnectionAcceptor compositionConnectionAcceptor = new CompositionConnectionAcceptor(acceptors);
            
            //___________________________//
            
            Server server = new Server(compositionConnectionAcceptor);
            
            server.Creative = creativeModeIsActive;
            
            server.Start();
            
            Debug.Log($"TimerSetByHostInMin : {GameConstants.TimerSetByHostInMin} min | PercentageSetByHost : {GameConstants.PercentageSetByHost} %");
            server.SetWinCondition(new WinCondition((uint)GameConstants.TimerSetByHostInMin*60, GameConstants.PercentageSetByHost/100d));
            
            //___________________________//
            
            (LocalConnection client, LocalConnection serverConnection) = LocalConnection.CreatePair();
            localAcceptor.Join(client);
            
            Server = server;
            
            FinaliseConnection(new Proxy(serverConnection, new GameActionListenerManager()));
            
            Debug.Log("Host Server");
        }

        public override void Stop()
        {
            if (Server != null)
            {
                Server.Stop();
            }
            Server = null;
            if (_lobbyID != null)
            {
                SteamMatchmaking.LeaveLobby(_lobbyID.Value);
            }
            _lobbyID = null;
            
            SteamConnectionMethod.Instance?.Stop();
        }
        
        private void SetupCallbacks()
        {
            Callback<LobbyCreated_t>.Create(param =>
            {
                Console.WriteLine("Lobby created requested " + param.m_ulSteamIDLobby);

                _lobbyID = new CSteamID(param.m_ulSteamIDLobby);
                
                SteamMatchmaking.SetLobbyData(_lobbyID.Value, "host_id",
                    SteamUser.GetSteamID().ToString());
                SteamFriends.SetRichPresence("connect", param.m_ulSteamIDLobby.ToString());
            });
            Callback<LobbyEnter_t>.Create(param =>
            {
                Console.WriteLine("Lobby entered requested " + param.m_ulSteamIDLobby);
                SteamFriends.SetRichPresence("steam_player_group", param.m_ulSteamIDLobby.ToString());
            });

            Callback<LobbyChatUpdate_t>.Create(param =>
            {
                Console.WriteLine("Lobby join requested " + param.m_ulSteamIDLobby);
                SteamFriends.SetRichPresence("steam_player_group_size",
                    SteamMatchmaking.GetNumLobbyMembers(new CSteamID(param.m_ulSteamIDLobby)).ToString());
            });
        }
        
        private void Add(List<IConnectionAcceptor> acceptors)
        {
            if (acceptTcpConnection)
            {
                AddTcp(acceptors);
            }

            if (SteamManager.Initialized && acceptSteamConnection)
            {
                AddSteam(acceptors);
            }
        }

        private void AddTcp(List<IConnectionAcceptor> acceptors)
        {
            acceptors.Add(new TcpConnectionAcceptor(12345, registry));
        }

        private void AddSteam(List<IConnectionAcceptor> acceptors)
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 10);
            acceptors.Add(new SteamConnectionAcceptor(registry, i => true));
        }
    }
}

public class MyDebug : TextWriter
{
    public override Encoding Encoding { get; }

    public override void WriteLine(string value)
    {
        Debug.Log("[SERVER]     :   " + value);
    }
}