using UnityEngine;

namespace BuildingsFolder.BuildingsClasses
{
    public class Extractor : Building
    {
        public enum ResourceType { Wood, Stone, Gold, Diamond, Obsidian, Water, Vine }

        public ResourceType ExtractedResource { get; private set; }

        private bool isExtracting = false;
        
        public Extractor(string name, string description, GameObject gameObject, (int, int) position, uint ownerId, ResourceType extractedResource, GameObject flag) : base(name, description, gameObject, position, ownerId, flag)
        {
            isExtracting = true;
            ExtractedResource = extractedResource;
        }
    }
}