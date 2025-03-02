using UnityEngine;

namespace BuildingsFolder.BuildingsClasses
{
    public class Building 
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public GameObject GameObject { get; private set; }
        public (int, int) Position { get; private set; }
        public uint OwnerId { get; private set; }

        public bool Paralyze;
        
        public GameObject Flag { get; private set; }

        public Building(string name, string description, GameObject gameObject, (int, int) position, uint ownerId, GameObject flag)
        {
            Name = name;
            Description = description;
            GameObject = gameObject;
            Position = position;
            OwnerId = ownerId;
            Flag = flag;
        }
    }
}