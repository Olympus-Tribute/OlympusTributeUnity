using OlympusWorldGenerator.Generators;
using UnityEngine;

namespace Grid
{
    public class GridGenerator : MonoBehaviour
    {
   
        public GameObject grassPrefabs;
        public GameObject oceanPrefabs;
        public GameObject woodresourcePrefabs;
        public GameObject diamondresourcePrefabs;
        public GameObject obsidianresourcePrefabs;
        public GameObject goldresourcePrefabs;
        public GameObject stoneresourcePrefabs;
        public GameObject vineresourcePrefabs;
        public GameObject lakePrefabs;
        
        public HexMapGenerator MapGenerator {get; private set;}

        public void Generate(int seed, uint mapwidth, uint mapheight)
        {
            PerlinFloorGenerator generator = new PerlinFloorGenerator(0.6f, 0.65f, 0.02f, 20, 3, 0.03f, 32, 0.8f);

            MapGenerator =  new HexMapGenerator(mapwidth, mapheight, grassPrefabs, oceanPrefabs,
                woodresourcePrefabs, diamondresourcePrefabs, obsidianresourcePrefabs, goldresourcePrefabs,
                stoneresourcePrefabs, vineresourcePrefabs, lakePrefabs, generator);
            MapGenerator.GridGeneration(this.gameObject,seed);
        }
    
    

    }
}

