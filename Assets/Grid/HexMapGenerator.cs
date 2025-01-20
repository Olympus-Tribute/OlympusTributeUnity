using UnityEngine;

public class HexMapGenerator : MonoBehaviour
{
    public int mapWidth = 10; // Nombre d'hexagones en largeur
    public int mapHeight = 10; // Nombre d'hexagones en hauteur
    public float hexWidth = 10f; // Largeur d'un hexagone
    public float hexHeight = 10f; // Hauteur d'un hexagone
    public GameObject[] hexPrefabs;
    public ETile[] Tiles;
    
    void Start()
    {
        Tiles = MapInitializator(mapWidth, mapHeight);
        GenerateHexMap();
    }

    void GenerateHexMap()
    {
        float xOffset = hexWidth * 2f; // Décalage horizontal 
        float zOffset = hexHeight + hexHeight / 2f; // Décalage vertical 

        for (int z = 0; z < mapWidth; z++)
        {
            for (int x = 0; x < mapHeight; x++)
            {
                float xPos = x * xOffset;
                float zPos = z * zOffset;

                // Décalage pour aligner les lignes impaires
                if (z % 2 == 1)
                {
                    xPos += xOffset / 2;
                }
                //Debug.Log($"l'hexagone apparait aux coordonnés {(x,z)} est en aux positions {(xPos,0,zPos)}");
                CreateHexTile(new Vector3(xPos, 0, zPos),EnumToIndex(Tiles[z*mapWidth + x]));
            }
        }
    }

    void CreateHexTile(Vector3 position, int prefabindex)
    {
        

        GameObject hexPrefab = hexPrefabs[prefabindex];

        // Instancier le modèle sélectionné
        GameObject hex = GameObject.Instantiate(hexPrefab, this.transform);
        Debug.Log($"un hexagon a spawn avec le prefab numéro {prefabindex} {hexPrefab}");
        hex.transform.position = position;

        // Facultatif : ajuster l'échelle ou la rotation si nécessaire
        hex.transform.localScale = new Vector3(1f, 1f, 1f);
        hex.transform.rotation = Quaternion.identity;
        
        
    }

    static int EnumToIndex(ETile tile)
    {
        if (tile is ETile.Grass)
            return 0;
        if (tile is ETile.Wood)
            return 1;
        if (tile is ETile.Diamond)
            return 2;
        if (tile is ETile.Water)
            return 3;
        if (tile is ETile.Obsidian)
            return 4;
        if (tile is ETile.Gold)
            return 5;
        if (tile is ETile.Stone)
            return 6;
        else
            return 7;

    }

    static ETile[] MapInitializator(int mapWidth,int mapHeight)
    {
        ETile[] res = new ETile[mapWidth * mapHeight];
        for (int z = 0; z < mapWidth; z++)
        {
            for (int x = 0; x < mapHeight; x++)
            {
                int randomNumber = Random.Range(0, 101);
                if (randomNumber < 40)
                    res[z * mapWidth + x] = ETile.Grass;
                else if (randomNumber < 45)
                    res[z * mapWidth + x] = ETile.Water;
                else if (randomNumber < 70)
                    res[z * mapWidth + x] = ETile.Wood;
                else if (randomNumber < 72)
                    res[z * mapWidth + x] = ETile.Diamond;
                else if (randomNumber < 75)
                    res[z * mapWidth + x] = ETile.Obsidian;
                else if (randomNumber < 80)
                    res[z * mapWidth + x] = ETile.Gold;
                else if (randomNumber < 85)
                    res[z * mapWidth + x] = ETile.Stone;
                else
                    res[z * mapWidth + x] = ETile.Vine;
            }
        }

        return res;
    }
    

    Mesh CreateHexMesh()
    {
        Mesh mesh = new Mesh();
        float halfWidth = hexWidth / 2f;
        float halfHeight = hexHeight / 2f;
        
        Vector3[] vertices = new Vector3[7]
        {
            new Vector3(0, 0, 0), // Centre 0
            new Vector3(0, 0, hexHeight), // A
            new Vector3(hexWidth, 0, halfHeight), // B
            new Vector3(hexWidth, 0, -halfHeight), // C
            new Vector3(0, 0, -hexHeight), // D
            new Vector3(-hexWidth, 0, -halfHeight), // E
            new Vector3(-hexWidth, 0, halfHeight) // F
        };

        int[] triangles = new int[18]
        {
            0, 1, 2,
            0, 2, 3,
            0, 3, 4,
            0, 4, 5,
            0, 5, 6,
            0, 6, 1
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
public enum ETile
{
    Grass,
    Wood,
    Diamond,
    Water,
    Obsidian,
    Gold,
    Stone,
    Vine
}