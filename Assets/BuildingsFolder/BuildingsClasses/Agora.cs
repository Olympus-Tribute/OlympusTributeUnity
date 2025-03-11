using UnityEngine;

namespace BuildingsFolder.BuildingsClasses
{
    public class Agora : Building
    {
        public Agora(string name, string description, GameObject gameObject, (int, int) position, uint ownerId, GameObject flag) : base(name, description, gameObject, position, ownerId, 5, flag)
        {
        }
    }
}