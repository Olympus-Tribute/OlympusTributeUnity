using System;
using OlympusWorldGenerator;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;


namespace Grid
{
    public class HexMapGenerator : IFloorGrid
    {
        private const float hexSize = 10f; // longueur d'un coté hexagone
        private const double sqrt3 = 1.7320508076d;
        private readonly uint mapWidth = 10; // Nombre d'hexagones en largeur
        private readonly uint mapHeight = 10; // Nombre d'hexagones en hauteur
    
        private readonly GameObject grassPrefabs;
        private readonly GameObject oceanPrefabs;
        private readonly GameObject woodresourcePrefabs;
        private readonly GameObject diamondresourcePrefabs;
        private readonly GameObject obsidianresourcePrefabs;
        private readonly GameObject goldresourcePrefabs;
        private readonly GameObject stoneresourcePrefabs;
        private readonly GameObject vineresourcePrefabs;
        private readonly GameObject lakePrefabs;
        
    
        private readonly FloorTile[] Tiles;
        private readonly IFloorGenerator Generator;
        
        public uint Width
        {
            get => mapWidth;
        }

        public uint Height
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
        public HexMapGenerator(uint mapWidth, uint mapHeight, GameObject grassPrefabs, GameObject waterPrefabs,
            GameObject woodresourcePrefabs, GameObject diamondresourcePrefabs, GameObject obsidianresourcePrefabs,
            GameObject goldresourcePrefabs, GameObject stoneresourcePrefabs, GameObject vineresourcePrefabs,
            GameObject waterresourcePrefabs, IFloorGenerator Generator)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.grassPrefabs = grassPrefabs;
            this.oceanPrefabs = waterPrefabs;
            this.woodresourcePrefabs = woodresourcePrefabs;
            this.diamondresourcePrefabs = diamondresourcePrefabs;
            this.obsidianresourcePrefabs = obsidianresourcePrefabs;
            this.goldresourcePrefabs = goldresourcePrefabs;
            this.stoneresourcePrefabs = stoneresourcePrefabs;
            this.vineresourcePrefabs = vineresourcePrefabs;
            this.lakePrefabs = waterresourcePrefabs;
            this.Generator = Generator;
            Tiles =  new FloorTile[mapWidth * mapHeight];
        }

        

        public void GridGeneration(GameObject parent, int seed)
        {
            Generator.Generate(this,seed);
            GenerateHexMap(parent,seed);
        }

        private void GenerateHexMap(GameObject parent, int seed)
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

                    CreateHexTile(parent,new Vector3((float)xPos, 0, (float)zPos),(this[x,z]), seed);
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
                case (FloorTile.Lake):
                    return lakePrefabs;
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
                case (FloorTile.Ocean):
                    return oceanPrefabs;
                default:
                    throw new ArgumentException("une tile qui n'existe pas essaie d'être instancié");
            }
        }

        public void CreateHexTile(GameObject parent, Vector3 position, FloorTile tile, int seed)
        {
        
            GameObject hexPrefab = EnumToPrefab(tile);

            // Instancier le modèle sélectionné
            GameObject hex = Object.Instantiate(hexPrefab, parent.transform);
            hex.transform.position = position;
        

            // Facultatif : ajuster l'échelle ou la rotation si nécessaire
            hex.transform.localScale = new Vector3(1f, 1f, 1f);
            Random random = new Random((int)(seed + position.x + position.z*Width));
            
            int randomvalue = random.Next(0, 6);
            hex.transform.rotation = Quaternion.Euler(0, randomvalue/6F * 360, 0);

        }
    }
}
