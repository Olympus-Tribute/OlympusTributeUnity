using System;
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
        
        public double LastTimeParalyzed = double.NegativeInfinity;
        public double LastParalyzedDuration = 0;

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
        
        public bool Usable(double now)
        {
            return now - LastTimeParalyzed >= LastTimeParalyzed;
        }

        public void Paralyze(double duration)
        {
            LastParalyzedDuration = duration;
            LastTimeParalyzed = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}