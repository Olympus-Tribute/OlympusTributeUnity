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
        public static Server Server { get; private set; }
        
        //______________________________________________________//
        //______________________________________________________//
        //______________________________________________________//

        public static long Seed { get; set; }
        
        public static uint PlayerCount { get; set; }
        
        public static uint PlayerId { get; set; }
        
        
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
            Server.Update(Time.deltaTime);
        }

        public void Host()
        {
            Debug.Log("Hosting Server....");
            Console.SetOut(new MyDebug());
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
    }
}

public class MyDebug : TextWriter
{
    public override Encoding Encoding { get; }

    public override void WriteLine(string value)
    {
        Debug.Log("[SERVER] :   " + value);
    }
}