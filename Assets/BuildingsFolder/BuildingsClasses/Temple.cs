using ForNetwork;
using Networking.Common.Client;
using OlympusDedicatedServer.Components.Attack;
using UnityEngine;

namespace BuildingsFolder.BuildingsClasses
{
    public class Temple : Building
    {
        public readonly AttackType Type;
        public string DescriptionAttack;
        
        public Temple(string name, string description, GameObject gameObject, (int, int) position, uint ownerId, AttackType type, string  descriptionAttack, GameObject flag) : base(name, description, gameObject, position, ownerId, 3, flag)
        {
            Type = type;
            DescriptionAttack = descriptionAttack;
        }

        public void SendAttack(int targetX, int targetY)
        {
            Network.Instance.Proxy.Connection.Send(new ClientAttackGameAction(targetX,targetY,Position.Item1,Position.Item2));
        }
    }
}
