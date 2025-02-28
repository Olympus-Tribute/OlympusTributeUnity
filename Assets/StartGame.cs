using ForNetwork;
using Networking.Common.Client;
using UnityEngine;

namespace DefaultNamespace
{
    public class StartGame : MonoBehaviour
    {
        public void OnEnable()
        {
            var action = new ClientDoneLoadingGameAction();
            Network.Instance.Proxy.Connection.Send(action);
        }
    }
}