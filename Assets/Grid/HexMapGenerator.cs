using System;
using OlympusWorldGenerator;
using OlympusWorldGenerator.Generators;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = System.Random;


public class HexMapGenerator : MonoBehaviour, IFloorGrid
{
    public int mapWidth = 10; // Nombre d'hexagones en largeur
    public int mapHeight = 10; // Nombre d'hexagones en hauteur
    public float hexSize = 10f; // longueur d'un coté hexagone
    public GameObject[] hexPrefabs;
    public FloorTile[] Tiles;
    public IFloorGenerator Generator = new RandomFloorGenerator(10, 10, 0, 10, 10, 10, 0, 0);
    
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
            Debug.Log($"accède à l'array {y * Width + x} de co {(x,y)}");
            Tiles[y * Width + x] = value;
        }
    }

    public FloorTile this[int i]
    {
        get => Tiles[i];
        set => Tiles[i] = value;
    }
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Tiles =  new FloorTile[mapWidth * mapHeight];
        int seed = 0;
        Generator.Generate(this,seed);
        GenerateHexMap(seed);
    }

    void GenerateHexMap(int seed)
    {
        float sqrt3 = (float)1.73205;
        float xOffset = sqrt3*hexSize; // Décalage horizontal 
        float zOffset = hexSize*3/2; // Décalage vertical 
        
        for (int z = 0; z < mapWidth; z++)
        {
            for (int x = 0; x < mapHeight; x++)
            {
                float xPos = x * xOffset;
                float zPos = z * zOffset;

                // Décalage pour aligner les lignes impaires
                if (z % 2 == 1)
                {
                    xPos += xOffset/2;
                }
                Debug.Log($"l'hexagone apparait aux coordonnés {(x,z)} est en aux positions {(xPos,zPos)}");
                CreateHexTile(new Vector3(xPos, 0, zPos),(int)(this[z,x]), seed);
            }
        }
    }

    void CreateHexTile(Vector3 position, int prefabindex, int seed)
    {
        //Debug.Log($"prefab index : {prefabindex}");
        
        GameObject hexPrefab = hexPrefabs[prefabindex];

        // Instancier le modèle sélectionné
        GameObject hex = GameObject.Instantiate(hexPrefab, this.transform);
        //Debug.Log($"un hexagon a spawn avec le prefab numéro {prefabindex} {hexPrefab}");
        hex.transform.position = position;
        

        // Facultatif : ajuster l'échelle ou la rotation si nécessaire
        hex.transform.localScale = new Vector3(1f, 1f, 1f);
        Random random = new Random((int)(seed + position.x + position.z));
        int randomvalue = random.Next(0, 6);
        switch (randomvalue)
        {
            case 0:
                hex.transform.rotation = Quaternion.Euler(0, 0, 0);
                Debug.Log($"tourné à 0°");
                break;
            case 1:
                hex.transform.rotation = Quaternion.Euler(0, 60, 0);
                Debug.Log($"tourné à 60°");
                break;
            case 2:
                hex.transform.rotation = Quaternion.Euler(0, 120, 0);
                Debug.Log($"tourné à 120°");
                break;
            case 3:
                hex.transform.rotation = Quaternion.Euler(0, 180, 0);
                Debug.Log($"tourné à 180°");
                break;
            case 4:
                hex.transform.rotation = Quaternion.Euler(0, 240, 0);
                Debug.Log($"tourné à 240°");
                break;
            case 5:
                hex.transform.rotation = Quaternion.Euler(0, 300, 0);
                Debug.Log($"tourné à 300°");
                break;
        }
    }
}
