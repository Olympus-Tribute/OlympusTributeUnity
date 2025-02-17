using System;
using OlympusWorldGenerator;
using OlympusWorldGenerator.Generators;
using UnityEngine;
using Random = System.Random;


namespace Grid
{
    public class HexMapGenerator : MonoBehaviour, IFloorGrid
    {
        private const float hexSize = 10f; // longueur d'un coté hexagone
        private const double sqrt3 = 1.7320508076d;
        private readonly int mapWidth = 10; // Nombre d'hexagones en largeur
        private readonly int mapHeight = 10; // Nombre d'hexagones en hauteur
    
        private readonly GameObject grassPrefabs;
        private readonly GameObject waterPrefabs;
        private readonly GameObject woodresourcePrefabs;
        private readonly GameObject diamondresourcePrefabs;
        private readonly GameObject obsidianresourcePrefabs;
        private readonly GameObject goldresourcePrefabs;
        private readonly GameObject stoneresourcePrefabs;
        private readonly GameObject vineresourcePrefabs;
        private readonly GameObject waterresourcePrefabs;
        
    
        private readonly FloorTile[] Tiles;
        private readonly IFloorGenerator Generator;
        
        public int Width
        {
            get => mapWidth;
        }

        public int Height
        {
            get => mapHeight;
        }

        public FloorTile this[int x, int y]
        {
            get => Tiles[y * Width + x];
            set
            {
                Tiles[y * Width + x] = value;
            }
        }

        public FloorTile this[int i]
        {
            get => Tiles[i];
            set => Tiles[i] = value;
        }
        public HexMapGenerator(int mapWidth, int mapHeight, GameObject grassPrefabs, GameObject waterPrefabs,
            GameObject woodresourcePrefabs, GameObject diamondresourcePrefabs, GameObject obsidianresourcePrefabs,
            GameObject goldresourcePrefabs, GameObject stoneresourcePrefabs, GameObject vineresourcePrefabs,
            GameObject waterresourcePrefabs, IFloorGenerator Generator)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.grassPrefabs = grassPrefabs;
            this.waterPrefabs = waterPrefabs;
            this.woodresourcePrefabs = woodresourcePrefabs;
            this.diamondresourcePrefabs = diamondresourcePrefabs;
            this.obsidianresourcePrefabs = obsidianresourcePrefabs;
            this.goldresourcePrefabs = goldresourcePrefabs;
            this.stoneresourcePrefabs = stoneresourcePrefabs;
            this.vineresourcePrefabs = vineresourcePrefabs;
            this.waterresourcePrefabs = waterresourcePrefabs;
            this.Generator = Generator;
            Tiles =  new FloorTile[mapWidth * mapHeight];
        }

        

        public void GridGeneration(int seed)
        {
            
            Generator.Generate(this,seed);
            GenerateHexMap(seed);
        }

        private void GenerateHexMap(int seed)
        {
            
            double xOffset = sqrt3*hexSize; // Décalage horizontal 
            double zOffset = hexSize*3/2; // Décalage vertical 
        
            for (int z = 0; z < mapWidth; z++)
            {
                for (int x = 0; x < mapHeight; x++)
                {
                    double xPos = x * xOffset;
                    double zPos = z * zOffset;

                    // Décalage pour aligner les lignes impaires
                    if (z % 2 == 1)
                    {
                        xPos += xOffset/2;
                    }
                    CreateHexTile(new Vector3((float)xPos, 0, (float)zPos),(this[z,x]), seed);
                }
            }
        }

        private GameObject EnumToPrefab(FloorTile tile)
        {
            switch (tile)
            {
                case(FloorTile.Grass):
                    return grassPrefabs;
                case (FloorTile.Wood):
                    return woodresourcePrefabs;
                case (FloorTile.Water):
                    return waterresourcePrefabs;
                case (FloorTile.DiamondMountain):
                    return diamondresourcePrefabs;
                case (FloorTile.ObsidianMountain):
                    return obsidianresourcePrefabs;
                case (FloorTile.GoldMountain):
                    return goldresourcePrefabs;
                case (FloorTile.StoneMountain):
                    return stoneresourcePrefabs;
                case (FloorTile.Vine):
                    return vineresourcePrefabs;
                default:
                    throw new ArgumentException("une tile qui n'existe pas essaie d'être instancié");
            }
        }

        private void CreateHexTile(Vector3 position, FloorTile tile, int seed)
        {
        
            GameObject hexPrefab = EnumToPrefab(tile);

            // Instancier le modèle sélectionné
            GameObject hex = Instantiate(hexPrefab, this.transform);
            hex.transform.position = position;
        

            // Facultatif : ajuster l'échelle ou la rotation si nécessaire
            hex.transform.localScale = new Vector3(1f, 1f, 1f);
            Random random = new Random((int)(seed + position.x + position.z*Width));
            int randomvalue = random.Next(0, 6);
            switch (randomvalue)
            {
                case 0:
                    hex.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 1:
                    hex.transform.rotation = Quaternion.Euler(0, 60, 0);
                    break;
                case 2:
                    hex.transform.rotation = Quaternion.Euler(0, 120, 0);
                    break;
                case 3:
                    hex.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 4:
                    hex.transform.rotation = Quaternion.Euler(0, 240, 0);
                    break;
                case 5:
                    hex.transform.rotation = Quaternion.Euler(0, 300, 0);
                    break;
            }
        }
    }
}
