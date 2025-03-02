using UnityEngine;

namespace BuildingsFolder.BuildingsClasses
{
    public class House : Building
    {
        public House(string name, string description, GameObject gameObject, (int, int) position, uint ownerId , GameObject flag) : base(name, description, gameObject, position, ownerId ,flag)
        {
        }
    }
}