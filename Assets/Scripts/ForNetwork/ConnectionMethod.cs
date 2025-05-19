using Networking.API.Actions;
using OlympusDedicatedServer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ForNetwork
{
    public abstract class ConnectionMethod : MonoBehaviour
    {
        protected readonly GameActionRegistry registry;

        public ConnectionMethod()
        {
            registry = GameActions.RegisterAll();
        }

        public abstract void Connect();
        public abstract void Stop();

        protected void FinaliseConnection(Proxy proxy)
        {
            proxy.Connection.Connect();
            Network.Instance.ActiveConnectionMethod = this;
            Network.Instance.Proxy = proxy;
            
            SceneManager.LoadScene("WaitingScene");
        }
    }
}