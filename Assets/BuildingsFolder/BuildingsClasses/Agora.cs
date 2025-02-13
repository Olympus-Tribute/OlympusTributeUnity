using UnityEngine;

namespace BuildingsFolder.BuildingsClasses
{
    public class Agora : Building
    {
        public Agora(string name, string description, GameObject gameObject, (int, int) position, ushort ownerId) : base(name, description, gameObject, position, ownerId)
        {
        }
    }
}