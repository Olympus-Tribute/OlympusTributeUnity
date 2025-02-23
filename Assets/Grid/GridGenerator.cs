using OlympusWorldGenerator;
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

        public void Generate(int seed, int mapwidth, int mapheight)
        {
            IFloorGenerator generator = new RandomFloorGenerator(40, 10, 0, 8, 8, 8, 10, 8,8);
            HexMapGenerator mapGenerator = new HexMapGenerator(mapwidth, mapheight, grassPrefabs, oceanPrefabs,
                woodresourcePrefabs, diamondresourcePrefabs, obsidianresourcePrefabs, goldresourcePrefabs,
                stoneresourcePrefabs, vineresourcePrefabs, lakePrefabs, generator);
            mapGenerator.GridGeneration(this.gameObject,seed);
        }
    }
}

