using System;
using ForNetwork;
using JetBrains.Annotations;
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
        public static ServerManager Instance {get; private set;}
        
        public static Server Server { get; private set; }
        
        public void Awake()
        {
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

        public void Update()
        {
            if (Server == null)
            {
                return;
            }
            Server.Update();
        }

        public void Host()
        {
            LocalConnectionAcceptor acceptor = new LocalConnectionAcceptor();
            
            Server server = new Server(acceptor);
            
            server.Start();
            
            (LocalConnection client, LocalConnection serverConnection) = LocalConnection.CreatePair();
            acceptor.Join(client);
            
            Server = server;
        
            Network.Instance.Proxy = new ForNetwork.Proxy(serverConnection, new GameActionListenerManager());
        }
    }
}