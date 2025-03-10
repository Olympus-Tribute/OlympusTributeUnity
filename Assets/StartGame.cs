using ForNetwork;
using Networking.Common.Client;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void OnEnable()
    {
        var action = new ClientDoneLoadingGameAction();
        Network.Instance.Proxy.Connection.Send(action);
    }
}