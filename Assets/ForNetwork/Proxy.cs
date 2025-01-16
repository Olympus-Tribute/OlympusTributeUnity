using Networking.API;
using Networking.API.Actions;
using Networking.API.Listeners;

namespace ForNetwork
{
    public class Proxy
    {
        public readonly IConnection Connection;
        public readonly GameActionListenerManager GameActionListenerManager;

        public Proxy(IConnection connection, GameActionListenerManager gameActionListenerManager)
        {
            Connection = connection;
            GameActionListenerManager = gameActionListenerManager;
        }

        public void Process()
        {
            Connection.Update();
            while (Connection.Receive() is { } gameAction) // = while (Connection.Receive() is not null, gameAction)
            {
                GameActionListenerManager.Notify(Connection, gameAction);
            }
        }
    }
}