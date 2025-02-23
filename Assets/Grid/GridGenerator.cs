using Grid;
using OlympusWorldGenerator;
using OlympusWorldGenerator.Generators;
using UnityEngine;

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

    public void Generate(int seed, uint mapwidth, uint mapheight)
    {
        IFloorGenerator Generator = new RandomFloorGenerator(40, 10, 0, 8, 8, 8, 10, 8,8);
        HexMapGenerator MapGenerator = new HexMapGenerator(mapwidth, mapheight, grassPrefabs, oceanPrefabs,
            woodresourcePrefabs, diamondresourcePrefabs, obsidianresourcePrefabs, goldresourcePrefabs,
            stoneresourcePrefabs, vineresourcePrefabs, lakePrefabs, Generator);
        MapGenerator.GridGeneration(this.gameObject,seed);
    }
    
    

}

