using System;
using System.IO;
using System.Text;
using ForNetwork;
using JetBrains.Annotations;
using Networking.API;
using Networking.API.Actions;
using Networking.API.Listeners;
using Networking.Common.Client;
using Networking.Common.Server;
using Networking.Local;
using Networking.Steam;
using OlympusDedicatedServer;
using Steamworks;
using UnityEngine;
using Proxy = OlympusDedicatedServer.Proxy;

namespace ForServer
{
    public class ServerManager : MonoBehaviour
    {
        //______________________________________________________//
        //______________________________________________________//
        //______________________________________________________//
        public static ServerManager Instance {get; private set;}
        public static Server Server { get; set; }
        
        //______________________________________________________//
        //______________________________________________________//
        //______________________________________________________//

        public static int Seed { get; set; }

        public static uint MapWidth { get; set; }
        
        public static uint MapHeight { get; set; }
        
        public static uint PlayerCount { get; set; }
        
        public static uint PlayerId { get; set; }
        

        private static CSteamID? lobbyID;
        
        
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Optionnel, pour garder l'instance entre les scènes
                SetupCallbacks();
            }
            else
            {
                Destroy(gameObject); // Évite les doublons
            }
        }

        public void Update()
        {
            if (Server == null)
            {
                return;
            }
            Server.Update(Time.deltaTime);
        }

        public void Host()
        {
            Debug.Log("Hosting Server....");
            Console.SetOut(new MyDebug());

            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 10);
            
            LocalConnectionAcceptor acceptor = new LocalConnectionAcceptor();

            
            SteamConnectionAcceptor steamConnectionAcceptor =
                new SteamConnectionAcceptor(Network.Instance.registry, (_) => true);
            
            
            IConnectionAcceptor[] allAcceptors = new IConnectionAcceptor[]{acceptor, steamConnectionAcceptor};
            CompositionConnectionAcceptor compositionConnectionAcceptor = new CompositionConnectionAcceptor(allAcceptors);
            
            
            Server server = new Server(compositionConnectionAcceptor);
            
            server.Start();
            
            (LocalConnection client, LocalConnection serverConnection) = LocalConnection.CreatePair();
            acceptor.Join(client);
            
            Server = server;
        
            Network.Instance.Proxy = new ForNetwork.Proxy(serverConnection, new GameActionListenerManager());

            Network.Instance.Setup(Network.Instance.Proxy);
            
            Debug.Log("Host Server");
        }

        public void StopHost()
        {
            Server = null;
            if (lobbyID != null)
            {
                SteamMatchmaking.LeaveLobby(lobbyID.Value);
            }
            lobbyID = null;
        }
        
        private static void SetupCallbacks()
        {
            
            Callback<LobbyCreated_t>.Create(param =>
            {
                Console.WriteLine("Lobby created requested " + param.m_ulSteamIDLobby);

                lobbyID = new CSteamID(param.m_ulSteamIDLobby);
                
                SteamMatchmaking.SetLobbyData(lobbyID.Value, "host_id",
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