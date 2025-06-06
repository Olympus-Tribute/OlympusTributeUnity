using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ForNetwork
{
    public class Network : MonoBehaviour
    {
    
        public Proxy Proxy; 
        
        public static Network Instance {get; private set;}
        
        [CanBeNull] public ConnectionMethod ActiveConnectionMethod;
        

        private Network()
        {
            Proxy = null;
        }

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
            if (Proxy == null)
            {
                return;
            }
            Proxy.Process();
            
            if (Proxy.Connection.IsConnected)
            {
                return;
            }

            Stop();
            SceneManager.LoadScene("MainMenu");
        }

        public void Stop()
        {
            Proxy?.Connection.Disconnect();
            ActiveConnectionMethod?.Stop();
            ActiveConnectionMethod = null;
            Proxy = null;
        }
    }
}