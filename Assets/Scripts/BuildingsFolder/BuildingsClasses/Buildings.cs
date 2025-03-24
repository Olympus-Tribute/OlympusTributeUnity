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

        public readonly int Range;
        
        public Building(string name, string description, GameObject gameObject, (int, int) position, uint ownerId,  int range)
        {
            Name = name;
            Description = description;
            GameObject = gameObject;
            Position = position;
            OwnerId = ownerId;
            Range = range;
        }
    }
}