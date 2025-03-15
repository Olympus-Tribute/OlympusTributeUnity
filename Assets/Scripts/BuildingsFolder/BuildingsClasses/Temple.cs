using System.Collections.Generic;
using ForNetwork;
using Networking.Common.Client;
using OlympusDedicatedServer.Components.Attack;
using Resources;
using UnityEngine;

namespace BuildingsFolder.BuildingsClasses
{
    public class Temple : Building
    {
        public readonly AttackType AttackType;
        public readonly string DescriptionAttack;
        public Dictionary<ResourceType, int> AttackPrice;
        
        public Temple(string name, string description, GameObject gameObject, (int, int) position, uint ownerId, AttackType attackType, Dictionary<ResourceType, int> attackPrice, string  descriptionAttack, GameObject flag) : base(name, description, gameObject, position, ownerId, 3, flag)
        {
            AttackType = attackType;
            DescriptionAttack = descriptionAttack;
            AttackPrice = attackPrice;
        }

        

        public void SendAttack(int targetX, int targetY)
        {
            Network.Instance.Proxy.Connection.Send(new ClientAttackGameAction(targetX,targetY,Position.Item1,Position.Item2));
        }
    }
}
