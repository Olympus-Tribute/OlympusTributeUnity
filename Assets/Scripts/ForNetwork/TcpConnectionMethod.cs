using Networking.API.Listeners;
using Networking.TCP;
using UnityEngine;

namespace ForNetwork
{
    public class TcpConnectionMethod : ConnectionMethod
    {
        public static TcpConnectionMethod Instance { get; private set; }
        
        public string hostIP = "127.0.0.1";
        
        public TcpConnectionMethod(){}
        
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public override void Connect()
        {
            if (Network.Instance.Proxy == null)
            {
                TcpConnection connection = TcpConnection.Connect(hostIP, 12345, registry);
                GameActionListenerManager gameActionListener = new GameActionListenerManager();
                
                FinaliseConnection(new Proxy(connection, gameActionListener));
            }
        }

        public override void Stop() {}
    }
}